import * as assert from 'assert';
import * as fs from 'fs';
import * as os from 'os';
import * as path from 'path';
import {
  listSections,
  buildActivityEntry,
  appendActivityToSection,
} from '../src/commands/createActivity';

function makeTempDir(): string {
  return fs.mkdtempSync(path.join(os.tmpdir(), 'cac-test-'));
}

describe('listSections', () => {
  it('returns empty array when Sections/ does not exist', () => {
    const root = makeTempDir();
    assert.deepStrictEqual(listSections(root), []);
    fs.rmSync(root, { recursive: true });
  });

  it('returns subdirectory paths under Sections/', () => {
    const root = makeTempDir();
    fs.mkdirSync(path.join(root, 'Sections', 'Introduction'), { recursive: true });
    fs.mkdirSync(path.join(root, 'Sections', 'Advanced'), { recursive: true });
    const sections = listSections(root);
    assert.ok(sections.includes('Sections/Introduction'));
    assert.ok(sections.includes('Sections/Advanced'));
    fs.rmSync(root, { recursive: true });
  });
});

describe('buildActivityEntry', () => {
  it('returns correct ActivityFilePath with sanitized name', () => {
    // buildActivityEntry takes raw name and sanitizes internally via buildFileName
    assert.strictEqual(buildActivityEntry('Forum', 'My Forum'), 'Activities/Forum_my-forum.yaml');
  });

  it('works with already-clean names', () => {
    assert.strictEqual(buildActivityEntry('Quiz', 'midterm'), 'Activities/Quiz_midterm.yaml');
  });
});

describe('appendActivityToSection', () => {
  it('creates Activities block when none exists', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Introduction');
    fs.mkdirSync(sectionDir, { recursive: true });
    fs.writeFileSync(path.join(sectionDir, 'section.yaml'), 'SectionName: Introduction\n');

    appendActivityToSection(sectionDir, 'Activities/Forum_test.yaml');

    const content = fs.readFileSync(path.join(sectionDir, 'section.yaml'), 'utf8');
    assert.ok(content.includes('Activities:'));
    assert.ok(content.includes('Forum_test.yaml'));
    fs.rmSync(root, { recursive: true });
  });

  it('appends to existing Activities block', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Introduction');
    fs.mkdirSync(sectionDir, { recursive: true });
    const existing = `SectionName: Introduction\nActivities:\n  - ActivityFilePath: Activities/Page_first.yaml\n`;
    fs.writeFileSync(path.join(sectionDir, 'section.yaml'), existing);

    appendActivityToSection(sectionDir, 'Activities/Forum_second.yaml');

    const content = fs.readFileSync(path.join(sectionDir, 'section.yaml'), 'utf8');
    assert.ok(content.includes('Page_first.yaml'));
    assert.ok(content.includes('Forum_second.yaml'));
    fs.rmSync(root, { recursive: true });
  });

  it('throws when section.yaml is unparseable', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Introduction');
    fs.mkdirSync(sectionDir, { recursive: true });
    fs.writeFileSync(path.join(sectionDir, 'section.yaml'), ': invalid: yaml: {{{\n');

    assert.throws(() => {
      appendActivityToSection(sectionDir, 'Activities/Forum_test.yaml');
    });
    fs.rmSync(root, { recursive: true });
  });
});
