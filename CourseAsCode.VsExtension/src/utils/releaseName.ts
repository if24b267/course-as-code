import * as fs from 'fs';
import * as path from 'path';

/**
 * Make a string safe to use as a filename component.
 *
 * Note: this sanitizer is distinct from `sanitizeName` in `templates.ts`, which
 * lowercases and uses hyphens for YAML activity filenames. This function
 * preserves case and uses underscores, and is intended for `.mbz` release filenames.
 */
export function sanitizeForFilename(name: string): string {
  return name
    .trim()
    // eslint-disable-next-line no-control-regex
    .replace(/[<>:"/\\|?*\x00-\x1F]/g, '_') // invalid filename characters
    .replace(/\s+/g, '_')                    // whitespace -> underscore
    .replace(/_+/g, '_')                     // collapse repeats
    .replace(/^_+|_+$/g, '');                // trim leading/trailing underscores
}

/**
 * Build a versioned release filename. First file for a (course, date) pair has
 * no suffix; subsequent ones get _2, _3, ... `existing` is a list of basenames.
 *
 * Note: `date` must already be a filesystem-safe, preformatted string such as
 * `YYYY-MM-DD`. It is injected into the filename verbatim without further sanitization.
 */
export function buildReleaseName(courseName: string, date: string, existing: string[]): string {
  const base = `${sanitizeForFilename(courseName)}_${date}`;
  const taken = new Set(existing);
  if (!taken.has(`${base}.mbz`)) {
    return `${base}.mbz`;
  }
  let n = 2;
  while (taken.has(`${base}_${n}.mbz`)) {
    n++;
  }
  return `${base}_${n}.mbz`;
}

/**
 * Absolute paths of all .mbz files in `outputDir` (empty if the dir is absent).
 *
 * Note: the returned order is unspecified (filesystem enumeration order).
 * Callers that need a specific ordering must sort the result themselves.
 */
export function listReleases(outputDir: string): string[] {
  if (!fs.existsSync(outputDir)) {
    return [];
  }
  return fs.readdirSync(outputDir)
    .filter(f => f.toLowerCase().endsWith('.mbz'))
    .map(f => path.join(outputDir, f));
}
