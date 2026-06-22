import * as assert from 'assert';
import * as fs from 'fs';
import * as os from 'os';
import * as path from 'path';
import {
  getQuestionPoolCompletions,
  getActivityCompletions,
  getSectionCompletions,
  RefCompletionProvider,
} from '../src/providers/refCompletionProvider';

function makeTempDir(): string {
  return fs.mkdtempSync(path.join(os.tmpdir(), 'cac-ref-test-'));
}

// Minimal vscode shim injected before each provideCompletionItems call
// eslint-disable-next-line @typescript-eslint/no-require-imports
const NodeModule = require('module') as { _load: Function };

function withVscodeMock(fn: () => void): void {
  const original = NodeModule._load;
  // Also clear any cached 'vscode' entry so our mock always wins
  const Module = NodeModule as any;
  const cacheKey = Object.keys(Module._cache ?? {}).find((k: string) => k.includes('vscode'));
  if (cacheKey) delete Module._cache[cacheKey];

  NodeModule._load = function (request: string, ...args: unknown[]) {
    if (request === 'vscode') {
      return {
        CompletionItemKind: { File: 17, Folder: 18 },
        CompletionItem: class MockItem {
          label: string;
          kind: number;
          insertText: string;
          constructor(label: string, kind: number) {
            this.label = label;
            this.kind = kind;
            this.insertText = label;
          }
        },
      };
    }
    return original.call(this, request, ...args);
  };
  try {
    fn();
  } finally {
    NodeModule._load = original;
  }
}

function mockDoc(fileName: string, lineText: string) {
  return { fileName, lineAt: (_line: number) => ({ text: lineText }) };
}

describe('getQuestionPoolCompletions', () => {
  it('returns relative paths without .yaml for all yaml files in questionpool/', () => {
    const root = makeTempDir();
    const qpDir = path.join(root, 'questionpool');
    fs.mkdirSync(qpDir, { recursive: true });
    fs.writeFileSync(path.join(qpDir, 'q1.yaml'), 'QuestionId: 1\n');
    fs.writeFileSync(path.join(qpDir, 'q2.yaml'), 'QuestionId: 2\n');
    const quizPath = path.join(root, 'Sections', 'S1', 'Activities', 'Quiz_test.yaml');
    fs.mkdirSync(path.dirname(quizPath), { recursive: true });

    const result = getQuestionPoolCompletions(quizPath);

    assert.strictEqual(result.length, 2);
    assert.ok(result.every(r => !r.endsWith('.yaml')), 'no .yaml extension');
    assert.ok(result.some(r => r.includes('q1')));
    assert.ok(result.some(r => r.includes('q2')));
    // Verify the returned path resolves back to the original file
    const firstResult = result.find(r => r.includes('q1'))!;
    const resolved = path.resolve(path.dirname(quizPath), firstResult + '.yaml');
    assert.strictEqual(resolved, path.join(qpDir, 'q1.yaml'));
    fs.rmSync(root, { recursive: true });
  });

  it('returns empty array when questionpool/ does not exist', () => {
    const root = makeTempDir();
    const quizPath = path.join(root, 'Sections', 'S1', 'Activities', 'Quiz_test.yaml');
    fs.mkdirSync(path.dirname(quizPath), { recursive: true });

    const result = getQuestionPoolCompletions(quizPath);

    assert.deepStrictEqual(result, []);
    fs.rmSync(root, { recursive: true });
  });
});

describe('getActivityCompletions', () => {
  it('returns yaml filenames from Activities/ next to section.yaml', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Intro');
    const activitiesDir = path.join(sectionDir, 'Activities');
    fs.mkdirSync(activitiesDir, { recursive: true });
    fs.writeFileSync(path.join(activitiesDir, 'Quiz_test.yaml'), '');
    fs.writeFileSync(path.join(activitiesDir, 'Page_intro.yaml'), '');
    const sectionYamlPath = path.join(sectionDir, 'section.yaml');

    const result = getActivityCompletions(sectionYamlPath);

    assert.ok(result.includes('Quiz_test.yaml'));
    assert.ok(result.includes('Page_intro.yaml'));
    fs.rmSync(root, { recursive: true });
  });

  it('returns empty array when Activities/ does not exist', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Intro');
    fs.mkdirSync(sectionDir, { recursive: true });
    const sectionYamlPath = path.join(sectionDir, 'section.yaml');

    const result = getActivityCompletions(sectionYamlPath);

    assert.deepStrictEqual(result, []);
    fs.rmSync(root, { recursive: true });
  });

  it('does not include non-yaml files from Activities/', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Intro');
    const activitiesDir = path.join(sectionDir, 'Activities');
    fs.mkdirSync(activitiesDir, { recursive: true });
    fs.writeFileSync(path.join(activitiesDir, 'Quiz_test.yaml'), '');
    fs.writeFileSync(path.join(activitiesDir, 'notes.md'), '');
    const sectionYamlPath = path.join(sectionDir, 'section.yaml');

    const result = getActivityCompletions(sectionYamlPath);

    assert.ok(result.includes('Quiz_test.yaml'));
    assert.ok(!result.includes('notes.md'));
    fs.rmSync(root, { recursive: true });
  });
});

describe('getSectionCompletions', () => {
  it('returns subdirectory names from Sections/ next to sections.yaml', () => {
    const root = makeTempDir();
    fs.mkdirSync(path.join(root, 'Sections', '01-intro'), { recursive: true });
    fs.mkdirSync(path.join(root, 'Sections', '02-basics'), { recursive: true });
    const sectionsYamlPath = path.join(root, 'sections.yaml');

    const result = getSectionCompletions(sectionsYamlPath);

    assert.ok(result.includes('01-intro'));
    assert.ok(result.includes('02-basics'));
    fs.rmSync(root, { recursive: true });
  });

  it('returns empty array when Sections/ does not exist', () => {
    const root = makeTempDir();
    const sectionsYamlPath = path.join(root, 'sections.yaml');

    const result = getSectionCompletions(sectionsYamlPath);

    assert.deepStrictEqual(result, []);
    fs.rmSync(root, { recursive: true });
  });

  it('does not include files (only directories) from Sections/', () => {
    const root = makeTempDir();
    fs.mkdirSync(path.join(root, 'Sections', '01-intro'), { recursive: true });
    fs.writeFileSync(path.join(root, 'Sections', 'README.md'), '');
    const sectionsYamlPath = path.join(root, 'sections.yaml');

    const result = getSectionCompletions(sectionsYamlPath);

    assert.ok(result.includes('01-intro'));
    assert.ok(!result.includes('README.md'));
    fs.rmSync(root, { recursive: true });
  });
});

describe('RefCompletionProvider - provideCompletionItems', () => {
  it('returns File completions for Quiz_*.yaml on a Ref line', () => {
    const root = makeTempDir();
    const qpDir = path.join(root, 'questionpool');
    fs.mkdirSync(qpDir, { recursive: true });
    fs.writeFileSync(path.join(qpDir, 'q1.yaml'), 'QuestionId: 1\n');
    const quizPath = path.join(root, 'Sections', 'S1', 'Activities', 'Quiz_test.yaml');
    fs.mkdirSync(path.dirname(quizPath), { recursive: true });

    withVscodeMock(() => {
      const provider = new RefCompletionProvider();
      const items = provider.provideCompletionItems(
        mockDoc(quizPath, '  - Ref: ') as any,
        { line: 0 } as any
      );
      assert.ok(items.length > 0, 'should return completions');
      assert.ok(items.every((i: any) => i.kind === 17), 'all File kind');
      assert.ok(items.every((i: any) => !(i.label as string).endsWith('.yaml')));
    });
    fs.rmSync(root, { recursive: true });
  });

  it('also triggers on the new QUIZ_*.yaml prefix on a Ref line', () => {
    const root = makeTempDir();
    const qpDir = path.join(root, 'questionpool');
    fs.mkdirSync(qpDir, { recursive: true });
    fs.writeFileSync(path.join(qpDir, 'q1.yaml'), 'QuestionId: 1\n');
    const quizPath = path.join(root, 'Sections', 'S1', 'Activities', 'QUIZ_test.yaml');
    fs.mkdirSync(path.dirname(quizPath), { recursive: true });

    withVscodeMock(() => {
      const provider = new RefCompletionProvider();
      const items = provider.provideCompletionItems(
        mockDoc(quizPath, '  - Ref: ') as any,
        { line: 0 } as any
      );
      assert.ok(items.length > 0, 'QUIZ_ prefix should return completions');
    });
    fs.rmSync(root, { recursive: true });
  });

  it('returns File completions with Activities/ insertText for section.yaml on ActivityFilePath line', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Intro');
    const activitiesDir = path.join(sectionDir, 'Activities');
    fs.mkdirSync(activitiesDir, { recursive: true });
    fs.writeFileSync(path.join(activitiesDir, 'Quiz_test.yaml'), '');
    const sectionYamlPath = path.join(sectionDir, 'section.yaml');

    withVscodeMock(() => {
      const provider = new RefCompletionProvider();
      const items = provider.provideCompletionItems(
        mockDoc(sectionYamlPath, '  ActivityFilePath: ') as any,
        { line: 0 } as any
      );
      assert.ok(items.length > 0);
      assert.ok(items.every((i: any) => i.kind === 17), 'File kind');
      assert.ok(items.every((i: any) => (i.insertText as string).startsWith('Activities/')));
    });
    fs.rmSync(root, { recursive: true });
  });

  it('returns Folder completions with Sections/ insertText for sections.yaml on SectionPath line', () => {
    const root = makeTempDir();
    fs.mkdirSync(path.join(root, 'Sections', '01-intro'), { recursive: true });
    const sectionsYamlPath = path.join(root, 'sections.yaml');

    withVscodeMock(() => {
      const provider = new RefCompletionProvider();
      const items = provider.provideCompletionItems(
        mockDoc(sectionsYamlPath, '  SectionPath: ') as any,
        { line: 0 } as any
      );
      assert.ok(items.length > 0);
      assert.ok(items.every((i: any) => i.kind === 18), 'Folder kind');
      assert.ok(items.every((i: any) => (i.insertText as string).startsWith('Sections/')));
    });
    fs.rmSync(root, { recursive: true });
  });

  it('returns empty array for an unrelated yaml file', () => {
    const root = makeTempDir();
    const otherPath = path.join(root, 'config.yaml');

    withVscodeMock(() => {
      const provider = new RefCompletionProvider();
      const items = provider.provideCompletionItems(
        mockDoc(otherPath, '  SomeProp: ') as any,
        { line: 0 } as any
      );
      assert.deepStrictEqual(items, []);
    });
    fs.rmSync(root, { recursive: true });
  });

  it('returns empty array for section.yaml on a non-ActivityFilePath line', () => {
    const root = makeTempDir();
    const sectionYamlPath = path.join(root, 'Sections', 'Intro', 'section.yaml');
    fs.mkdirSync(path.dirname(sectionYamlPath), { recursive: true });

    withVscodeMock(() => {
      const provider = new RefCompletionProvider();
      const items = provider.provideCompletionItems(
        mockDoc(sectionYamlPath, '  SectionName: ') as any,
        { line: 0 } as any
      );
      assert.deepStrictEqual(items, []);
    });
    fs.rmSync(root, { recursive: true });
  });

  it('returns empty array for sections.yaml on a non-SectionPath line', () => {
    const root = makeTempDir();
    const sectionsYamlPath = path.join(root, 'sections.yaml');

    withVscodeMock(() => {
      const provider = new RefCompletionProvider();
      const items = provider.provideCompletionItems(
        mockDoc(sectionsYamlPath, '  SomethingElse: ') as any,
        { line: 0 } as any
      );
      assert.deepStrictEqual(items, []);
    });
    fs.rmSync(root, { recursive: true });
  });
});
