import * as assert from 'assert';
import { QUESTION_TYPES, getQuestionTemplate, QuestionType } from '../src/questionTemplates';

describe('questionTemplates', () => {
  it('exposes all 13 Moodle question types', () => {
    assert.strictEqual(QUESTION_TYPES.length, 13);
  });

  it('every type has a template whose QuestionType matches the picked type', () => {
    for (const type of QUESTION_TYPES) {
      const tpl = getQuestionTemplate(type);
      assert.ok(
        tpl.includes(`QuestionType: ${type}`),
        `template for ${type} should declare "QuestionType: ${type}"`
      );
    }
  });

  it('every template starts with QuestionName', () => {
    for (const type of QUESTION_TYPES) {
      assert.ok(
        getQuestionTemplate(type).startsWith('QuestionName:'),
        `template for ${type} should start with QuestionName`
      );
    }
  });

  it('choice-style templates include an Answers list', () => {
    const withAnswers: QuestionType[] = ['SINGLECHOICE', 'MULTIPLECHOICE', 'TRUEFALSE', 'SHORTANSWER'];
    for (const type of withAnswers) {
      assert.ok(getQuestionTemplate(type).includes('Answers:'), `${type} should include Answers:`);
    }
  });
});
