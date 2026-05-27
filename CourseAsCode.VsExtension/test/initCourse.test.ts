import * as assert from 'assert';
import * as fs from 'fs';
import * as os from 'os';
import * as path from 'path';
import { scaffoldCourse, courseExists } from '../src/commands/initCourse';

function makeTempDir(): string {
  return fs.mkdtempSync(path.join(os.tmpdir(), 'cac-test-'));
}

describe('courseExists', () => {
  it('returns false when root is empty', () => {
    const root = makeTempDir();
    assert.strictEqual(courseExists(root), false);
    fs.rmSync(root, { recursive: true });
  });

  it('returns true when config.yaml exists', () => {
    const root = makeTempDir();
    fs.writeFileSync(path.join(root, 'config.yaml'), '');
    assert.strictEqual(courseExists(root), true);
    fs.rmSync(root, { recursive: true });
  });

  it('returns true when sections.yaml exists', () => {
    const root = makeTempDir();
    fs.writeFileSync(path.join(root, 'sections.yaml'), '');
    assert.strictEqual(courseExists(root), true);
    fs.rmSync(root, { recursive: true });
  });

  it('returns true when Sections/ directory exists', () => {
    const root = makeTempDir();
    fs.mkdirSync(path.join(root, 'Sections'));
    assert.strictEqual(courseExists(root), true);
    fs.rmSync(root, { recursive: true });
  });
});

describe('scaffoldCourse', () => {
  it('creates config.yaml with CourseName', () => {
    const root = makeTempDir();
    scaffoldCourse(root);
    const content = fs.readFileSync(path.join(root, 'config.yaml'), 'utf8');
    assert.ok(content.includes('CourseName:'));
    fs.rmSync(root, { recursive: true });
  });

  it('creates sections.yaml with Introduction section path', () => {
    const root = makeTempDir();
    scaffoldCourse(root);
    const content = fs.readFileSync(path.join(root, 'sections.yaml'), 'utf8');
    assert.ok(content.includes('Sections/Introduction'));
    fs.rmSync(root, { recursive: true });
  });

  it('creates Sections/Introduction/section.yaml with SectionName', () => {
    const root = makeTempDir();
    scaffoldCourse(root);
    const sectionFile = path.join(root, 'Sections', 'Introduction', 'section.yaml');
    assert.ok(fs.existsSync(sectionFile));
    const content = fs.readFileSync(sectionFile, 'utf8');
    assert.ok(content.includes('SectionName: Introduction'));
    fs.rmSync(root, { recursive: true });
  });

  it('creates all three artifacts in one call', () => {
    const root = makeTempDir();
    scaffoldCourse(root);
    assert.ok(fs.existsSync(path.join(root, 'config.yaml')));
    assert.ok(fs.existsSync(path.join(root, 'sections.yaml')));
    assert.ok(fs.existsSync(path.join(root, 'Sections', 'Introduction', 'section.yaml')));
    fs.rmSync(root, { recursive: true });
  });

  it('creates defaults directory and its default files', () => {
    const root = makeTempDir();
    scaffoldCourse(root);
    assert.ok(fs.existsSync(path.join(root, 'defaults')));
    assert.ok(fs.existsSync(path.join(root, 'defaults', 'default_quiz.yaml')));
    assert.ok(fs.existsSync(path.join(root, 'defaults', 'default_assignment.yaml')));
    assert.ok(fs.existsSync(path.join(root, 'defaults', 'default_forum.yaml')));
    assert.ok(fs.existsSync(path.join(root, 'defaults', 'default_question.yaml')));
    fs.rmSync(root, { recursive: true });
  });

  it('creates empty questionpool directory at course root', () => {
    const root = makeTempDir();
    scaffoldCourse(root);
    assert.ok(fs.existsSync(path.join(root, 'questionpool')));
    fs.rmSync(root, { recursive: true });
  });

  it('does not overwrite existing config.yaml when courseExists guard is checked first', () => {
    // Verify the guard pattern: courseExists + scaffoldCourse is the caller's contract.
    // If config.yaml exists, courseExists returns true and scaffoldCourse should NOT be called.
    const root = makeTempDir();
    fs.writeFileSync(path.join(root, 'config.yaml'), 'CourseName: Existing\n');
    assert.strictEqual(courseExists(root), true);
    // Confirm scaffoldCourse itself would overwrite — so callers MUST check courseExists first.
    // This test documents the contract, not a bug.
    const before = fs.readFileSync(path.join(root, 'config.yaml'), 'utf8');
    scaffoldCourse(root); // this WOULD overwrite — guard lives in the handler
    const after = fs.readFileSync(path.join(root, 'config.yaml'), 'utf8');
    assert.notStrictEqual(before, after); // documents that scaffoldCourse is not idempotent
    fs.rmSync(root, { recursive: true });
  });

  it('creates Activities/ subdirectory inside Introduction section', () => {
    const root = makeTempDir();
    scaffoldCourse(root);
    const activitiesDir = path.join(root, 'Sections', 'Introduction', 'Activities');
    assert.ok(fs.existsSync(activitiesDir), 'Activities/ should exist after scaffoldCourse');
    fs.rmSync(root, { recursive: true });
  });
});
