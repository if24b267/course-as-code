import * as assert from 'assert';
import * as fs from 'fs';
import * as os from 'os';
import * as path from 'path';
import { nextQuestionId, withQuestionId } from '../src/commands/createQuestion';

function makeTempDir(): string {
  return fs.mkdtempSync(path.join(os.tmpdir(), 'cac-q-'));
}

describe('nextQuestionId', () => {
  it('returns 1 for a missing or empty questionpool', () => {
    const dir = makeTempDir();
    assert.strictEqual(nextQuestionId(path.join(dir, 'nope')), 1);
    assert.strictEqual(nextQuestionId(dir), 1);
    fs.rmSync(dir, { recursive: true });
  });

  it('returns max existing QuestionId + 1', () => {
    const dir = makeTempDir();
    fs.writeFileSync(path.join(dir, 'a.yaml'), 'QuestionName: "a"\nQuestionId: 3\n');
    fs.writeFileSync(path.join(dir, 'b.yaml'), 'QuestionName: "b"\nQuestionId: 7\n');
    fs.writeFileSync(path.join(dir, 'c.yaml'), 'QuestionName: "c"\nQuestionId: 5\n');
    assert.strictEqual(nextQuestionId(dir), 8);
    fs.rmSync(dir, { recursive: true });
  });

  it('ignores non-yaml files', () => {
    const dir = makeTempDir();
    fs.writeFileSync(path.join(dir, 'a.yaml'), 'QuestionId: 2\n');
    fs.writeFileSync(path.join(dir, 'notes.txt'), 'QuestionId: 99\n');
    assert.strictEqual(nextQuestionId(dir), 3);
    fs.rmSync(dir, { recursive: true });
  });
});

describe('withQuestionId', () => {
  it('inserts a QuestionId line right after QuestionName', () => {
    const out = withQuestionId('QuestionName: ""\nQuestionType: ESSAY\n', 4);
    assert.strictEqual(out, 'QuestionName: ""\nQuestionId: 4\nQuestionType: ESSAY\n');
  });

  it('replaces an existing QuestionId line instead of duplicating it', () => {
    const out = withQuestionId('QuestionName: ""\nQuestionId: 0\nQuestionType: ESSAY\n', 9);
    assert.ok(out.includes('QuestionId: 9'));
    assert.ok(!out.includes('QuestionId: 0'));
    assert.strictEqual((out.match(/QuestionId:/g) || []).length, 1);
  });
});
