const fetch = require('node-fetch');
const jsdom = require('jsdom');
const TurndownService = require('turndown');
const fs = require('fs-extra');
const leftPad = require('left-pad');
const mkdirp = require('mkdirp-promise');

const argv = require('yargs')
    .usage('Usage: $0 [options]')
    .alias('s', 'session')
    .describe('s', 'Provide session cookie')
    .demandOption(['s'])
    .help('h')
    .alias('h', 'help')
    .argv;

const { JSDOM } = jsdom;
const turndownService = new TurndownService();
turndownService.addRule('hidden-span', {
    filter: ['span'],
    replacement: (content, node, options) => `${content} _(${node.getAttribute('title')})_`
});

let base = Promise.resolve();

for (let year = 2015; year <= 2019; year++) {
    const yearPath = `./${year}`;
    base = base.then(() => {
        return fs.pathExists(yearPath);
    })
        .then(exists => {
            if (!exists) {
                console.log(`Creating directory: ${yearPath}`);
                return mkdirp(yearPath);
            } else {
                return Promise.resolve();
            }
        });
    for (let day = 1; day <= 25; day++) {
        const dayPath = `${yearPath}/Day${leftPad(day, 2, 0)}.md`;
        base = base.then(() => {
            return fs.pathExists(dayPath);
        })
            .then(exists => {
                if (!exists) {
                    console.log(`Fetching questions for ${year} day ${day}`);
                    return fetchQuestion(year, day, dayPath)
                } else {
                    return Promise.resolve();
                }
            });
    }
}

return base;

function fetchQuestion(year, day, path) {
    return fetch(
        `http://adventofcode.com/${year}/day/${day}`,
        {
            headers: {
                cookie: `session=${argv.session}`
            },
        })
        .then(res => res.text())
        .then(html => {
            const dom = new JSDOM(html);
            return [...dom.window.document.querySelectorAll('article.day-desc')]
                .map(d => d.innerHTML)
                .map(h => turndownService.turndown(h))
                .join("\n\n");
        })
        .then(markdown => {
            return fs.writeFile(path, markdown);
        })
        .catch(() => console.log(`Error fetching ${year} day ${day}`));
}