import * as assert from 'assert';
import * as fs from 'fs';
import * as os from 'os';
import * as path from 'path';
import { buildSectionPath, appendSectionEntry } from '../../src/commands/createSection';

describe('createSection', () => {
  describe('buildSectionPath', () => {
    it('lowercases and replaces spaces with hyphens', () => {
      assert.strictEqual(buildSectionPath('Week 3'), 'Sections/week-3');
    });

    it('strips special characters', () => {
      assert.strictEqual(buildSectionPath('Week 3 - Loops!'), 'Sections/week-3---loops');
    });

    it('handles already clean names', () => {
      assert.strictEqual(buildSectionPath('introduction'), 'Sections/introduction');
    });
  });

  describe('appendSectionEntry', () => {
    let tmpDir: string;

    beforeEach(() => {
      tmpDir = fs.mkdtempSync(path.join(os.tmpdir(), 'cac-test-'));
    });

    afterEach(() => {
      fs.rmSync(tmpDir, { recursive: true, force: true });
    });

    it('appends entry to existing sections list', () => {
      const sectionsFile = path.join(tmpDir, 'sections.yaml');
      fs.writeFileSync(
        sectionsFile,
        'Sections:\n  - SectionPath: Sections/Introduction\n',
        'utf8'
      );

      appendSectionEntry(sectionsFile, 'Sections/week-2');

      // eslint-disable-next-line @typescript-eslint/no-require-imports
      const yaml = require('js-yaml');
      const parsed = yaml.load(fs.readFileSync(sectionsFile, 'utf8')) as {
        Sections: Array<{ SectionPath: string }>;
      };
      assert.strictEqual(parsed.Sections.length, 2);
      assert.strictEqual(parsed.Sections[1].SectionPath, 'Sections/week-2');
    });

    it('handles empty Sections array', () => {
      const sectionsFile = path.join(tmpDir, 'sections.yaml');
      fs.writeFileSync(sectionsFile, 'Sections: []\n', 'utf8');

      appendSectionEntry(sectionsFile, 'Sections/introduction');

      // eslint-disable-next-line @typescript-eslint/no-require-imports
      const yaml = require('js-yaml');
      const parsed = yaml.load(fs.readFileSync(sectionsFile, 'utf8')) as {
        Sections: Array<{ SectionPath: string }>;
      };
      assert.strictEqual(parsed.Sections.length, 1);
      assert.strictEqual(parsed.Sections[0].SectionPath, 'Sections/introduction');
    });

    it('handles null-parsed sections.yaml', () => {
      const sectionsFile = path.join(tmpDir, 'sections.yaml');
      fs.writeFileSync(sectionsFile, '{}\n', 'utf8');

      appendSectionEntry(sectionsFile, 'Sections/intro');

      // eslint-disable-next-line @typescript-eslint/no-require-imports
      const yaml = require('js-yaml');
      const parsed = yaml.load(fs.readFileSync(sectionsFile, 'utf8')) as {
        Sections: Array<{ SectionPath: string }>;
      };
      assert.strictEqual(parsed.Sections.length, 1);
    });
  });
});
