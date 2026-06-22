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

  it('prefixes Page files with PGE_', () => {
    assert.strictEqual(buildFileName('Page', 'welcome'), 'PGE_welcome.yaml');
  });

  it('prefixes Quiz files with QUIZ_', () => {
    assert.strictEqual(buildFileName('Quiz', 'midterm'), 'QUIZ_midterm.yaml');
  });

  it('prefixes Assignment files with ASN_', () => {
    assert.strictEqual(buildFileName('Assignment', 'homework'), 'ASN_homework.yaml');
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

  it('Quiz template includes Questions', () => {
    assert.ok(getTemplate('Quiz').includes('Questions:'));
  });

  it('Page template includes Content field', () => {
    assert.ok(getTemplate('Page').includes('Content:'));
  });
});

describe('getTemplate — all types have Type field', () => {
  it('Url template starts with Type: URL', () => {
    assert.ok(getTemplate('Url').startsWith('Type: URL'));
  });

  it('File template starts with Type: FILE', () => {
    assert.ok(getTemplate('File').startsWith('Type: FILE'));
  });

  it('Label template starts with Type: LABEL', () => {
    assert.ok(getTemplate('Label').startsWith('Type: LABEL'));
  });

  it('Folder template starts with Type: FOLDER', () => {
    assert.ok(getTemplate('Folder').startsWith('Type: FOLDER'));
  });

  it('Wiki template starts with Type: WIKI', () => {
    assert.ok(getTemplate('Wiki').startsWith('Type: WIKI'));
  });

  it('Choice template starts with Type: CHOICE', () => {
    assert.ok(getTemplate('Choice').startsWith('Type: CHOICE'));
  });

  it('Feedback template starts with Type: FEEDBACK', () => {
    assert.ok(getTemplate('Feedback').startsWith('Type: FEEDBACK'));
  });

  it('Choice template includes Options:', () => {
    assert.ok(getTemplate('Choice').includes('Options:'));
  });

  it('Feedback template includes Items:', () => {
    assert.ok(getTemplate('Feedback').includes('Items:'));
  });
});

describe('buildFileName — all activity type prefixes', () => {
  it('Url uses URL_ prefix', () => {
    assert.strictEqual(buildFileName('Url', 'my-link'), 'URL_my-link.yaml');
  });

  it('File uses FILE_ prefix', () => {
    assert.strictEqual(buildFileName('File', 'handout'), 'FILE_handout.yaml');
  });

  it('Label uses TMA_ prefix', () => {
    assert.strictEqual(buildFileName('Label', 'intro-text'), 'TMA_intro-text.yaml');
  });

  it('Folder uses FLD_ prefix', () => {
    assert.strictEqual(buildFileName('Folder', 'resources'), 'FLD_resources.yaml');
  });

  it('Wiki uses WKI_ prefix', () => {
    assert.strictEqual(buildFileName('Wiki', 'course-wiki'), 'WKI_course-wiki.yaml');
  });

  it('Choice uses CHO_ prefix', () => {
    assert.strictEqual(buildFileName('Choice', 'favorite-activity'), 'CHO_favorite-activity.yaml');
  });

  it('Feedback uses FBK_ prefix', () => {
    assert.strictEqual(buildFileName('Feedback', 'rate-this-course'), 'FBK_rate-this-course.yaml');
  });
});
