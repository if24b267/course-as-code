import * as assert from 'assert';
import * as fs from 'fs';
import * as os from 'os';
import * as path from 'path';
import { validateSections, scaffoldMissingSection, getMissingSections, ValidationResult } from '../src/commands/updateSections';

function makeTempDir(): string {
  return fs.mkdtempSync(path.join(os.tmpdir(), 'cac-test-'));
}

describe('validateSections', () => {
  it('returns error when sections.yaml does not exist', () => {
    const root = makeTempDir();
    const results = validateSections(root);
    assert.strictEqual(results.length, 1);
    assert.strictEqual(results[0].type, 'error');
    assert.ok(results[0].message.includes('sections.yaml'));
    fs.rmSync(root, { recursive: true });
  });

  it('returns success when all sections are valid', () => {
    const root = makeTempDir();
    fs.writeFileSync(path.join(root, 'sections.yaml'),
      'Sections:\n  - SectionPath: Sections/Introduction\n');
    const introDir = path.join(root, 'Sections', 'Introduction');
    fs.mkdirSync(introDir, { recursive: true });
    fs.writeFileSync(path.join(introDir, 'section.yaml'), 'SectionName: Introduction\n');
    const results = validateSections(root);
    assert.strictEqual(results.length, 1);
    assert.strictEqual(results[0].type, 'success');
    fs.rmSync(root, { recursive: true });
  });

  it('returns warning when section directory is missing', () => {
    const root = makeTempDir();
    fs.writeFileSync(path.join(root, 'sections.yaml'),
      'Sections:\n  - SectionPath: Sections/Advanced\n');
    const results = validateSections(root);
    const warnings = results.filter(r => r.type === 'warning');
    assert.ok(warnings.some(w => w.message.includes('Sections/Advanced')));
    fs.rmSync(root, { recursive: true });
  });

  it('returns warning when section.yaml is missing from directory', () => {
    const root = makeTempDir();
    fs.writeFileSync(path.join(root, 'sections.yaml'),
      'Sections:\n  - SectionPath: Sections/Introduction\n');
    fs.mkdirSync(path.join(root, 'Sections', 'Introduction'), { recursive: true });
    const results = validateSections(root);
    const warnings = results.filter(r => r.type === 'warning');
    assert.ok(warnings.some(w => w.message.includes('section.yaml')));
    fs.rmSync(root, { recursive: true });
  });

  it('reports one warning per missing item', () => {
    const root = makeTempDir();
    fs.writeFileSync(path.join(root, 'sections.yaml'),
      'Sections:\n  - SectionPath: Sections/A\n  - SectionPath: Sections/B\n');
    const results = validateSections(root);
    const warnings = results.filter(r => r.type === 'warning');
    assert.strictEqual(warnings.length, 2);
    fs.rmSync(root, { recursive: true });
  });
});

describe('scaffoldMissingSection', () => {
  it('creates the section directory', () => {
    const root = makeTempDir();
    scaffoldMissingSection(root, 'Sections/GettingStarted');
    assert.ok(fs.existsSync(path.join(root, 'Sections', 'GettingStarted')));
    fs.rmSync(root, { recursive: true });
  });

  it('creates section.yaml with correct SectionName', () => {
    const root = makeTempDir();
    scaffoldMissingSection(root, 'Sections/GettingStarted');
    const content = fs.readFileSync(
      path.join(root, 'Sections', 'GettingStarted', 'section.yaml'), 'utf8'
    );
    assert.ok(content.includes('SectionName: GettingStarted'));
    fs.rmSync(root, { recursive: true });
  });

  it('creates Activities subdirectory', () => {
    const root = makeTempDir();
    scaffoldMissingSection(root, 'Sections/GettingStarted');
    assert.ok(fs.existsSync(path.join(root, 'Sections', 'GettingStarted', 'Activities')));
    fs.rmSync(root, { recursive: true });
  });
});

describe('getMissingSections', () => {
  it('returns empty array when sections.yaml does not exist', () => {
    const root = makeTempDir();
    assert.deepStrictEqual(getMissingSections(root), []);
    fs.rmSync(root, { recursive: true });
  });

  it('returns paths for sections whose directories do not exist', () => {
    const root = makeTempDir();
    fs.writeFileSync(path.join(root, 'sections.yaml'),
      'Sections:\n  - SectionPath: Sections/A\n  - SectionPath: Sections/B\n');
    const missing = getMissingSections(root);
    assert.deepStrictEqual(missing, ['Sections/A', 'Sections/B']);
    fs.rmSync(root, { recursive: true });
  });

  it('excludes sections that already exist on disk', () => {
    const root = makeTempDir();
    fs.writeFileSync(path.join(root, 'sections.yaml'),
      'Sections:\n  - SectionPath: Sections/A\n  - SectionPath: Sections/B\n');
    fs.mkdirSync(path.join(root, 'Sections', 'A'), { recursive: true });
    const missing = getMissingSections(root);
    assert.deepStrictEqual(missing, ['Sections/B']);
    fs.rmSync(root, { recursive: true });
  });
});

describe('getMissingSections + validateSections interaction', () => {
  it('getMissingSections does not catch a section dir that exists but has no section.yaml', () => {
    const root = makeTempDir();
    // Section A: dir missing entirely
    // Section B: dir exists but no section.yaml
    fs.writeFileSync(
      path.join(root, 'sections.yaml'),
      'Sections:\n  - SectionPath: Sections/A\n  - SectionPath: Sections/B\n'
    );
    fs.mkdirSync(path.join(root, 'Sections', 'B'), { recursive: true });

    const missing = getMissingSections(root);
    assert.deepStrictEqual(missing, ['Sections/A']); // B is not in missing (dir exists)

    // After scaffolding A, validateSections should catch B's missing section.yaml
    const root2 = root;
    scaffoldMissingSection(root2, 'Sections/A');
    const results = validateSections(root2);
    const warnings = results.filter(r => r.type === 'warning');
    assert.ok(warnings.some(w => w.message.includes('Sections/B')));
    fs.rmSync(root, { recursive: true });
  });
});
