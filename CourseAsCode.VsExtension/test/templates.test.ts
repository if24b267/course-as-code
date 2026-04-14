import * as assert from 'assert';
import { buildFileName, getTemplate, sanitizeName, ActivityType } from '../src/templates';

describe('sanitizeName', () => {
  it('lowercases input', () => {
    assert.strictEqual(sanitizeName('MyForum'), 'myforum');
  });

  it('replaces spaces with hyphens', () => {
    assert.strictEqual(sanitizeName('My Forum'), 'my-forum');
  });

  it('strips characters that are not letters, digits, hyphens, or underscores', () => {
    assert.strictEqual(sanitizeName('hello/world!'), 'helloworld');
  });

  it('returns empty string for all-invalid input', () => {
    assert.strictEqual(sanitizeName('!!!'), '');
  });
});

describe('buildFileName', () => {
  it('prefixes Forum files with Forum_', () => {
    assert.strictEqual(buildFileName('Forum', 'general-discussion'), 'Forum_general-discussion.yaml');
  });

  it('prefixes Page files with Page_', () => {
    assert.strictEqual(buildFileName('Page', 'welcome'), 'Page_welcome.yaml');
  });

  it('prefixes Quiz files with Quiz_', () => {
    assert.strictEqual(buildFileName('Quiz', 'midterm'), 'Quiz_midterm.yaml');
  });

  it('prefixes Assignment files with Assignment_', () => {
    assert.strictEqual(buildFileName('Assignment', 'homework'), 'Assignment_homework.yaml');
  });

  it('sanitizes the name when it contains spaces', () => {
    assert.strictEqual(buildFileName('Forum', 'My Forum'), 'Forum_my-forum.yaml');
  });

  it('sanitizes the name when it contains invalid characters', () => {
    assert.strictEqual(buildFileName('Forum', 'hello/world!'), 'Forum_helloworld.yaml');
  });
});

describe('getTemplate', () => {
  it('Forum template starts with Type: FORUM', () => {
    assert.ok(getTemplate('Forum').startsWith('Type: FORUM'));
  });

  it('Page template starts with Type: PAGE', () => {
    assert.ok(getTemplate('Page').startsWith('Type: PAGE'));
  });

  it('Quiz template starts with Type: QUIZ', () => {
    assert.ok(getTemplate('Quiz').startsWith('Type: QUIZ'));
  });

  it('Assignment template starts with Type: ASSIGNMENT', () => {
    assert.ok(getTemplate('Assignment').startsWith('Type: ASSIGNMENT'));
  });

  it('Quiz template includes QuestionIds', () => {
    assert.ok(getTemplate('Quiz').includes('QuestionIds:'));
  });

  it('Page template includes Content field', () => {
    assert.ok(getTemplate('Page').includes('Content:'));
  });
});
