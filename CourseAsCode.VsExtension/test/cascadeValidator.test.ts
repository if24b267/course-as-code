import * as assert from 'assert';
import * as fs from 'fs';
import * as os from 'os';
import * as path from 'path';
import { findCourseRoot, getParentDefaultsPaths, getTopLevelKeys } from '../src/utils/cascadeValidator';

function makeTempDir(): string {
  return fs.mkdtempSync(path.join(os.tmpdir(), 'cac-validator-test-'));
}

describe('cascadeValidator helpers', () => {
  let tempRoot: string;
  let courseRoot: string;

  beforeEach(() => {
    tempRoot = makeTempDir();
    courseRoot = path.join(tempRoot, 'my-course');
    fs.mkdirSync(courseRoot);
    fs.writeFileSync(path.join(courseRoot, 'config.yaml'), 'CourseName: Test\n');
  });

  afterEach(() => {
    fs.rmSync(tempRoot, { recursive: true });
  });

  describe('findCourseRoot', () => {
    it('returns course root if config.yaml exists in parent chain', () => {
      const activityDir = path.join(courseRoot, 'Sections', 'Section1', 'Activities');
      fs.mkdirSync(activityDir, { recursive: true });
      const activityFile = path.join(activityDir, 'Quiz_test.yaml');

      const found = findCourseRoot(activityFile);
      assert.ok(found);
      assert.strictEqual(path.resolve(found), path.resolve(courseRoot));
    });

    it('returns null if config.yaml does not exist in parent chain', () => {
      const outsideDir = path.join(tempRoot, 'outside');
      fs.mkdirSync(outsideDir);
      const outsideFile = path.join(outsideDir, 'file.yaml');

      const found = findCourseRoot(outsideFile);
      assert.strictEqual(found, null);
    });
  });

  describe('getTopLevelKeys', () => {
    it('correctly extracts top level keys', () => {
      const file = path.join(courseRoot, 'test.yaml');
      fs.writeFileSync(file, `Type: QUIZ\nName: "My Quiz"\nQuestions:\n  - Ref: test\n`);

      const keys = getTopLevelKeys(file);
      assert.ok(keys.has('Type'));
      assert.ok(keys.has('Name'));
      assert.ok(keys.has('Questions'));
      assert.ok(!keys.has('Ref')); // nested key, should not be included
    });
  });

  describe('getParentDefaultsPaths', () => {
    it('finds specific and generic default files cascading up', () => {
      const section1Dir = path.join(courseRoot, 'Sections', 'Section1');
      const activitiesDir = path.join(section1Dir, 'Activities');
      fs.mkdirSync(activitiesDir, { recursive: true });

      // Create defaults folders
      const courseDefaults = path.join(courseRoot, 'defaults');
      const sectionDefaults = path.join(section1Dir, 'defaults');
      fs.mkdirSync(courseDefaults);
      fs.mkdirSync(sectionDefaults);

      // Create defaults files
      const courseQuizDefault = path.join(courseDefaults, 'default_quiz.yaml');
      const courseGenericDefault = path.join(courseDefaults, 'default.yaml');
      const sectionQuizDefault = path.join(sectionDefaults, 'default_quiz.yaml');
      
      fs.writeFileSync(courseQuizDefault, 'ShuffleQuestions: true\n');
      fs.writeFileSync(courseGenericDefault, 'DescriptionFormat: MARKDOWN\n');
      fs.writeFileSync(sectionQuizDefault, 'AllowedAttempts: 2\n');

      const activityFile = path.join(activitiesDir, 'Quiz_myquiz.yaml');
      
      const parentDefaults = getParentDefaultsPaths(activityFile, courseRoot);

      // Expected order: closest first, specific before generic
      // Path 1: section specific
      // Path 2: section generic (does not exist, skipped)
      // Path 3: course specific
      // Path 4: course generic
      assert.strictEqual(parentDefaults.length, 3);
      assert.strictEqual(path.resolve(parentDefaults[0]), path.resolve(sectionQuizDefault));
      assert.strictEqual(path.resolve(parentDefaults[1]), path.resolve(courseQuizDefault));
      assert.strictEqual(path.resolve(parentDefaults[2]), path.resolve(courseGenericDefault));
    });

    it('prevents defaults files from validating against themselves', () => {
      const section1Dir = path.join(courseRoot, 'Sections', 'Section1');
      fs.mkdirSync(path.join(section1Dir, 'defaults'), { recursive: true });

      const courseDefaults = path.join(courseRoot, 'defaults');
      fs.mkdirSync(courseDefaults);
      const courseQuizDefault = path.join(courseDefaults, 'default_quiz.yaml');
      fs.writeFileSync(courseQuizDefault, 'ShuffleQuestions: true\n');

      const sectionQuizDefault = path.join(section1Dir, 'defaults', 'default_quiz.yaml');
      fs.writeFileSync(sectionQuizDefault, 'AllowedAttempts: 2\n');

      const parentDefaults = getParentDefaultsPaths(sectionQuizDefault, courseRoot);

      // For sectionQuizDefault, it should find courseQuizDefault, but NOT sectionQuizDefault itself
      assert.strictEqual(parentDefaults.length, 1);
      assert.strictEqual(path.resolve(parentDefaults[0]), path.resolve(courseQuizDefault));
    });

    it('determines typeSuffix as question for files in questionpool', () => {
      const questionpoolDir = path.join(courseRoot, 'questionpool');
      fs.mkdirSync(questionpoolDir, { recursive: true });

      const courseDefaults = path.join(courseRoot, 'defaults');
      fs.mkdirSync(courseDefaults, { recursive: true });

      const courseQuestionDefault = path.join(courseDefaults, 'default_question.yaml');
      fs.writeFileSync(courseQuestionDefault, 'QuestionReady: true\n');

      const questionFile = path.join(questionpoolDir, 'question1.yaml');
      const parentDefaults = getParentDefaultsPaths(questionFile, courseRoot);

      assert.strictEqual(parentDefaults.length, 1);
      assert.strictEqual(path.resolve(parentDefaults[0]), path.resolve(courseQuestionDefault));
    });
  });
});
