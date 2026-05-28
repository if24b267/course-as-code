import * as assert from 'assert';
import * as fs from 'fs';
import * as os from 'os';
import * as path from 'path';
import { checkSectionActivities, checkQuizRefs } from '../src/utils/cascadeValidator';

function makeTempDir(): string {
  return fs.mkdtempSync(path.join(os.tmpdir(), 'cac-test-'));
}

describe('checkSectionActivities', () => {
  it('returns no findings when all ActivityFilePaths exist', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Intro');
    const activitiesDir = path.join(sectionDir, 'Activities');
    fs.mkdirSync(activitiesDir, { recursive: true });
    fs.writeFileSync(path.join(activitiesDir, 'Forum_test.yaml'), 'Type: FORUM\nName: test\n');
    const sectionYamlPath = path.join(sectionDir, 'section.yaml');
    const content = 'SectionName: Intro\nActivities:\n  - ActivityFilePath: Activities/Forum_test.yaml\n';
    fs.writeFileSync(sectionYamlPath, content);

    const findings = checkSectionActivities(sectionYamlPath, content);
    assert.strictEqual(findings.length, 0);
    fs.rmSync(root, { recursive: true });
  });

  it('returns error finding when ActivityFilePath does not exist on disk', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Intro');
    fs.mkdirSync(sectionDir, { recursive: true });
    const sectionYamlPath = path.join(sectionDir, 'section.yaml');
    const content = 'SectionName: Intro\nActivities:\n  - ActivityFilePath: Activities/Page_missing.yaml\n';
    fs.writeFileSync(sectionYamlPath, content);

    const findings = checkSectionActivities(sectionYamlPath, content);
    assert.strictEqual(findings.length, 1);
    assert.strictEqual(findings[0].severity, 'error');
    assert.ok(findings[0].message.includes('Page_missing.yaml'));
    fs.rmSync(root, { recursive: true });
  });

  it('returns no findings when Activities list is empty', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Intro');
    fs.mkdirSync(sectionDir, { recursive: true });
    const sectionYamlPath = path.join(sectionDir, 'section.yaml');
    const content = 'SectionName: Intro\nActivities: []\n';
    fs.writeFileSync(sectionYamlPath, content);

    const findings = checkSectionActivities(sectionYamlPath, content);
    assert.strictEqual(findings.length, 0);
    fs.rmSync(root, { recursive: true });
  });

  it('returns no findings when Activities key is absent', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Intro');
    fs.mkdirSync(sectionDir, { recursive: true });
    const sectionYamlPath = path.join(sectionDir, 'section.yaml');
    const content = 'SectionName: Intro\n';
    fs.writeFileSync(sectionYamlPath, content);

    const findings = checkSectionActivities(sectionYamlPath, content);
    assert.strictEqual(findings.length, 0);
    fs.rmSync(root, { recursive: true });
  });

  it('returns no findings when content is malformed YAML', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'Intro');
    fs.mkdirSync(sectionDir, { recursive: true });
    const sectionYamlPath = path.join(sectionDir, 'section.yaml');

    const findings = checkSectionActivities(sectionYamlPath, ': invalid: yaml: {{{');
    assert.strictEqual(findings.length, 0);
    fs.rmSync(root, { recursive: true });
  });
});

describe('checkQuizRefs', () => {
  it('returns no findings when Ref file exists and has QuestionId', () => {
    const root = makeTempDir();
    const questionPoolDir = path.join(root, 'questionpool');
    fs.mkdirSync(questionPoolDir, { recursive: true });
    fs.writeFileSync(
      path.join(questionPoolDir, 'q1.yaml'),
      'QuestionId: 1\nQuestionText: What is 1+1?\n'
    );
    const sectionDir = path.join(root, 'Sections', 'S1');
    const activitiesDir = path.join(sectionDir, 'Activities');
    fs.mkdirSync(activitiesDir, { recursive: true });
    const quizPath = path.join(activitiesDir, 'Quiz_test.yaml');
    const content = 'Type: QUIZ\nName: Test\nQuestions:\n  - Ref: "../../../questionpool/q1"\n';
    fs.writeFileSync(quizPath, content);

    const findings = checkQuizRefs(quizPath, content);
    assert.strictEqual(findings.length, 0);
    fs.rmSync(root, { recursive: true });
  });

  it('returns error when Ref file does not exist', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'S1');
    const activitiesDir = path.join(sectionDir, 'Activities');
    fs.mkdirSync(activitiesDir, { recursive: true });
    const quizPath = path.join(activitiesDir, 'Quiz_test.yaml');
    const content = 'Type: QUIZ\nName: Test\nQuestions:\n  - Ref: "../../../questionpool/missing-q"\n';
    fs.writeFileSync(quizPath, content);

    const findings = checkQuizRefs(quizPath, content);
    assert.strictEqual(findings.length, 1);
    assert.strictEqual(findings[0].severity, 'error');
    assert.ok(findings[0].message.includes('missing-q'));
    fs.rmSync(root, { recursive: true });
  });

  it('returns warning when Ref file exists but has no QuestionId field', () => {
    const root = makeTempDir();
    const questionPoolDir = path.join(root, 'questionpool');
    fs.mkdirSync(questionPoolDir, { recursive: true });
    fs.writeFileSync(
      path.join(questionPoolDir, 'bad-q.yaml'),
      'QuestionText: Missing the QuestionId field\n'
    );
    const sectionDir = path.join(root, 'Sections', 'S1');
    const activitiesDir = path.join(sectionDir, 'Activities');
    fs.mkdirSync(activitiesDir, { recursive: true });
    const quizPath = path.join(activitiesDir, 'Quiz_test.yaml');
    const content = 'Type: QUIZ\nName: Test\nQuestions:\n  - Ref: "../../../questionpool/bad-q"\n';
    fs.writeFileSync(quizPath, content);

    const findings = checkQuizRefs(quizPath, content);
    assert.strictEqual(findings.length, 1);
    assert.strictEqual(findings[0].severity, 'warning');
    assert.ok(findings[0].message.includes('bad-q'));
    fs.rmSync(root, { recursive: true });
  });

  it('returns no findings when QuestionId is 0 (falsy but valid)', () => {
    const root = makeTempDir();
    const questionPoolDir = path.join(root, 'questionpool');
    fs.mkdirSync(questionPoolDir, { recursive: true });
    fs.writeFileSync(
      path.join(questionPoolDir, 'q-zero.yaml'),
      'QuestionId: 0\nQuestionText: Valid question\n'
    );
    const sectionDir = path.join(root, 'Sections', 'S1');
    const activitiesDir = path.join(sectionDir, 'Activities');
    fs.mkdirSync(activitiesDir, { recursive: true });
    const quizPath = path.join(activitiesDir, 'Quiz_test.yaml');
    const content = 'Type: QUIZ\nName: Test\nQuestions:\n  - Ref: "../../../questionpool/q-zero"\n';
    fs.writeFileSync(quizPath, content);

    const findings = checkQuizRefs(quizPath, content);
    assert.strictEqual(findings.length, 0);
    fs.rmSync(root, { recursive: true });
  });

  it('returns no findings when Questions list is empty', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'S1');
    const activitiesDir = path.join(sectionDir, 'Activities');
    fs.mkdirSync(activitiesDir, { recursive: true });
    const quizPath = path.join(activitiesDir, 'Quiz_test.yaml');
    const content = 'Type: QUIZ\nName: Test\nQuestions: []\n';
    fs.writeFileSync(quizPath, content);

    const findings = checkQuizRefs(quizPath, content);
    assert.strictEqual(findings.length, 0);
    fs.rmSync(root, { recursive: true });
  });

  it('returns no findings for malformed YAML', () => {
    const root = makeTempDir();
    const sectionDir = path.join(root, 'Sections', 'S1');
    const activitiesDir = path.join(sectionDir, 'Activities');
    fs.mkdirSync(activitiesDir, { recursive: true });
    const quizPath = path.join(activitiesDir, 'Quiz_test.yaml');

    const findings = checkQuizRefs(quizPath, ': invalid: yaml: {{{');
    assert.strictEqual(findings.length, 0);
    fs.rmSync(root, { recursive: true });
  });
});
