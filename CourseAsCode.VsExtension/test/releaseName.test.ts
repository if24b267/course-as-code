import * as assert from 'assert';
import * as fs from 'fs';
import * as os from 'os';
import * as path from 'path';
import { sanitizeForFilename, buildReleaseName, listReleases } from '../src/utils/releaseName';

function makeTempDir(): string {
  return fs.mkdtempSync(path.join(os.tmpdir(), 'cac-rel-'));
}

describe('sanitizeForFilename', () => {
  it('replaces spaces with underscores', () => {
    assert.strictEqual(sanitizeForFilename('My Course'), 'My_Course');
  });

  it('replaces invalid filename characters with underscores', () => {
    assert.strictEqual(sanitizeForFilename('A/B:C*?'), 'A_B_C');
  });

  it('collapses repeats and trims leading/trailing underscores', () => {
    assert.strictEqual(sanitizeForFilename('  a   b  '), 'a_b');
  });

  it('returns empty string for all-invalid or empty input', () => {
    assert.strictEqual(sanitizeForFilename(''), '');
    assert.strictEqual(sanitizeForFilename('   '), '');
  });
});

describe('buildReleaseName', () => {
  it('uses base name when no collision', () => {
    assert.strictEqual(
      buildReleaseName('My Course', '2026-06-15', []),
      'My_Course_2026-06-15.mbz'
    );
  });

  it('appends _2 on first collision', () => {
    assert.strictEqual(
      buildReleaseName('My Course', '2026-06-15', ['My_Course_2026-06-15.mbz']),
      'My_Course_2026-06-15_2.mbz'
    );
  });

  it('finds next free index across multiple collisions', () => {
    assert.strictEqual(
      buildReleaseName('My Course', '2026-06-15', [
        'My_Course_2026-06-15.mbz',
        'My_Course_2026-06-15_2.mbz',
        'My_Course_2026-06-15_3.mbz',
      ]),
      'My_Course_2026-06-15_4.mbz'
    );
  });
});

describe('listReleases', () => {
  it('returns empty array when output dir does not exist', () => {
    assert.deepStrictEqual(listReleases(path.join(os.tmpdir(), 'cac-nope-xyz')), []);
  });

  it('returns absolute paths of .mbz files only', () => {
    const dir = makeTempDir();
    fs.writeFileSync(path.join(dir, 'a.mbz'), 'x');
    fs.writeFileSync(path.join(dir, 'notes.txt'), 'x');
    const result = listReleases(dir).map(p => path.basename(p));
    assert.deepStrictEqual(result, ['a.mbz']);
    fs.rmSync(dir, { recursive: true });
  });

  it('matches the .mbz extension case-insensitively', () => {
    const dir = makeTempDir();
    fs.writeFileSync(path.join(dir, 'Course.MBZ'), 'x');
    const result = listReleases(dir).map(p => path.basename(p));
    assert.deepStrictEqual(result, ['Course.MBZ']);
    fs.rmSync(dir, { recursive: true });
  });
});
