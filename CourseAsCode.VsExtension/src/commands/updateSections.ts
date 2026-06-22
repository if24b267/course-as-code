import type * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';
import type * as yamlTypes from 'js-yaml';
import { getWorkspaceRoot } from '../utils/workspace';

export type ValidationResult = {
  type: 'error' | 'warning' | 'success';
  message: string;
};

interface SectionsYaml {
  Sections?: Array<{ SectionPath: string }>;
}

function sectionNameFromPath(sectionPath: string): string {
  return path.basename(sectionPath);
}

export function scaffoldMissingSection(root: string, sectionPath: string, displayName?: string): void {
  const absDir = path.join(root, sectionPath);
  const sectionName = displayName ?? sectionNameFromPath(sectionPath);

  fs.mkdirSync(absDir, { recursive: true });
  fs.mkdirSync(path.join(absDir, 'Activities'), { recursive: true });
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const yaml = require('js-yaml') as typeof yamlTypes;
  fs.writeFileSync(
    path.join(absDir, 'section.yaml'),
    yaml.dump({ SectionName: sectionName }),
    'utf8'
  );
}

export function validateSections(root: string): ValidationResult[] {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const yaml = require('js-yaml') as typeof yamlTypes;

  const sectionsFile = path.join(root, 'sections.yaml');

  if (!fs.existsSync(sectionsFile)) {
    return [{
      type: 'error',
      message: 'No sections.yaml found in workspace root. Run Init Course first.',
    }];
  }

  let parsed: SectionsYaml | null;
  try {
    const raw = fs.readFileSync(sectionsFile, 'utf8');
    parsed = yaml.load(raw) as SectionsYaml | null;
  } catch (err) {
    return [{
      type: 'error',
      message: `sections.yaml could not be parsed: ${err instanceof Error ? err.message : String(err)}`,
    }];
  }

  const sections = parsed?.Sections ?? [];

  if (!Array.isArray(sections)) {
    return [{
      type: 'error',
      message: 'sections.yaml is malformed: "Sections" must be a list.',
    }];
  }

  const results: ValidationResult[] = [];

  for (const entry of sections) {
    const sectionPath = entry?.SectionPath;
    if (typeof sectionPath !== 'string' || !sectionPath) {
      results.push({ type: 'warning', message: 'sections.yaml contains an entry with a missing or invalid SectionPath.' });
      continue;
    }

    const absDir = path.join(root, sectionPath);

    if (!fs.existsSync(absDir)) {
      results.push({ type: 'warning', message: `Missing directory: ${sectionPath}` });
    } else if (!fs.existsSync(path.join(absDir, 'section.yaml'))) {
      results.push({ type: 'warning', message: `Missing section.yaml in: ${sectionPath}` });
    }
  }

  if (results.length === 0) {
    results.push({ type: 'success', message: 'All sections are valid.' });
  }

  return results;
}

export function getMissingSections(root: string): string[] {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const yaml = require('js-yaml') as typeof yamlTypes;

  const sectionsFile = path.join(root, 'sections.yaml');
  if (!fs.existsSync(sectionsFile)) {
    return [];
  }

  let parsed: SectionsYaml | null;
  try {
    const raw = fs.readFileSync(sectionsFile, 'utf8');
    parsed = yaml.load(raw) as SectionsYaml | null;
  } catch {
    // Corrupt sections.yaml — return empty, validateSections will surface the error
    return [];
  }

  const sections = parsed?.Sections ?? [];
  if (!Array.isArray(sections)) {
    return [];
  }

  return sections
    .filter(e => typeof e?.SectionPath === 'string' && e.SectionPath.length > 0)
    .map(e => e.SectionPath)
    .filter(p => !fs.existsSync(path.join(root, p)));
}

export async function updateSectionsCommand(): Promise<void> {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;

  const root = getWorkspaceRoot();
  if (!root) {
    vs.window.showErrorMessage('No workspace folder open.');
    return;
  }

  // Check for corrupt/missing sections.yaml upfront
  const sectionsFile = path.join(root, 'sections.yaml');
  if (!fs.existsSync(sectionsFile)) {
    vs.window.showErrorMessage('No sections.yaml found. Run Init Course first.');
    return;
  }

  let missing: string[];
  try {
    missing = getMissingSections(root);
  } catch (err) {
    vs.window.showErrorMessage(
      `Failed to read sections.yaml: ${err instanceof Error ? err.message : String(err)}`
    );
    return;
  }

  if (missing.length > 0) {
    const answer = await vs.window.showWarningMessage(
      `${missing.length} section(s) missing on disk. Create them now?`,
      'Create',
      'Cancel'
    );

    if (answer !== 'Create') {
      return;
    }

    const failed: string[] = [];
    for (const sectionPath of missing) {
      try {
        scaffoldMissingSection(root, sectionPath);
      } catch (err) {
        failed.push(sectionPath);
        console.error(
          `CourseAsCode: failed to scaffold ${sectionPath} — ${err instanceof Error ? err.message : String(err)}`
        );
      }
    }

    const created = missing.length - failed.length;
    if (created > 0) {
      vs.window.showInformationMessage(
        `Created ${created} section(s): ${missing.filter(p => !failed.includes(p)).map(p => path.basename(p)).join(', ')}`
      );
    }
    if (failed.length > 0) {
      vs.window.showErrorMessage(
        `Failed to create ${failed.length} section(s): ${failed.map(p => path.basename(p)).join(', ')}`
      );
    }
  }

  let results: ValidationResult[];
  try {
    results = validateSections(root);
  } catch (err) {
    vs.window.showErrorMessage(
      `Validation failed unexpectedly: ${err instanceof Error ? err.message : String(err)}`
    );
    return;
  }

  for (const result of results) {
    if (result.type === 'error') {
      vs.window.showErrorMessage(result.message);
    } else if (result.type === 'warning') {
      vs.window.showWarningMessage(result.message);
    } else {
      vs.window.showInformationMessage(result.message);
    }
  }
}