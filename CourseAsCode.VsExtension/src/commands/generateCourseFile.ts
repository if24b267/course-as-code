import type * as vscode from 'vscode';
import type * as yamlTypes from 'js-yaml';
import { spawn } from 'child_process';
import * as fs from 'fs';
import * as os from 'os';
import * as path from 'path';
import { buildReleaseName, listReleases } from '../utils/releaseName';
import { getWorkspaceRoot } from '../utils/workspace';

export interface SpawnResult {
  code: number | null;
  stdout: string;
  stderr: string;
}

export type Spawner = (command: string, args: string[]) => Promise<SpawnResult>;

/** Real spawner: runs a child process and captures stdout/stderr. */
export const realSpawner: Spawner = (command, args) =>
  new Promise((resolve, reject) => {
    const child = spawn(command, args, { shell: false });
    let stdout = '';
    let stderr = '';
    child.stdout.on('data', d => { stdout += d.toString(); });
    child.stderr.on('data', d => { stderr += d.toString(); });
    child.on('error', err => reject(err));
    child.on('close', code => resolve({ code, stdout, stderr }));
  });

/** Read CourseName from <root>/config.yaml, or undefined if absent. */
export function readCourseName(root: string): string | undefined {
  const configPath = path.join(root, 'config.yaml');
  if (!fs.existsSync(configPath)) {
    return undefined;
  }
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const yaml = require('js-yaml') as typeof yamlTypes;
  const parsed = yaml.load(fs.readFileSync(configPath, 'utf8')) as { CourseName?: string } | null;
  return parsed?.CourseName;
}

/** Return the single .mbz file in `dir`, throwing if there are zero or many. */
export function findSingleMbz(dir: string): string {
  const mbz = fs.readdirSync(dir).filter(f => f.toLowerCase().endsWith('.mbz'));
  if (mbz.length === 0) {
    throw new Error('No .mbz file was produced.');
  }
  if (mbz.length > 1) {
    throw new Error(`Expected one .mbz file but found ${mbz.length}.`);
  }
  return path.join(dir, mbz[0]);
}

export interface GenerateOptions {
  root: string;
  dllPath: string;
  courseName: string;
  date: string;
  spawn?: Spawner;
}

/**
 * Run the console app into a temp staging dir, then copy the produced .mbz into
 * <root>/output/ under a versioned name. Returns the final .mbz path.
 */
export async function generateMbz(opts: GenerateOptions): Promise<string> {
  const spawnFn = opts.spawn ?? realSpawner;
  const outputDir = path.join(opts.root, 'output');
  const staging = fs.mkdtempSync(path.join(os.tmpdir(), 'cac-stage-'));
  try {
    let result: SpawnResult;
    try {
      result = await spawnFn('dotnet', [opts.dllPath, '-i', opts.root, '-o', staging]);
    } catch (err) {
      const e = err as NodeJS.ErrnoException;
      if (e.code === 'ENOENT') {
        throw new Error(
          'The .NET 8 runtime is required to generate course files. Please install it and try again.'
        );
      }
      throw err;
    }

    if (result.code !== 0 || /(^|\n)Error:/.test(result.stdout)) {
      const tail = (result.stderr || result.stdout).trim().split(/\r?\n/).slice(-5).join('\n');
      throw new Error(`Course generation failed.\n${tail}`);
    }

    const producedMbz = findSingleMbz(staging);
    fs.mkdirSync(outputDir, { recursive: true });
    const existing = listReleases(outputDir).map(p => path.basename(p));
    const targetName = buildReleaseName(opts.courseName, opts.date, existing);
    const targetPath = path.join(outputDir, targetName);
    // copy (not rename) — staging is on the temp drive, possibly a different volume
    fs.copyFileSync(producedMbz, targetPath);
    return targetPath;
  } finally {
    fs.rmSync(staging, { recursive: true, force: true });
  }
}

function todayIso(): string {
  const d = new Date();
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  const dd = String(d.getDate()).padStart(2, '0');
  return `${d.getFullYear()}-${mm}-${dd}`;
}

export async function generateCourseFileCommand(extensionPath: string): Promise<void> {
  // eslint-disable-next-line @typescript-eslint/no-require-imports
  const vs = require('vscode') as typeof vscode;

  const root = getWorkspaceRoot();
  if (!root) {
    vs.window.showErrorMessage('No workspace folder open.');
    return;
  }

  const courseName = readCourseName(root);
  if (!courseName) {
    vs.window.showErrorMessage('Not a CourseAsCode course (config.yaml not found or missing CourseName).');
    return;
  }

  const dllPath = path.join(extensionPath, 'app', 'CourseAsCode.dll');
  if (!fs.existsSync(dllPath)) {
    vs.window.showErrorMessage('Bundled CourseAsCode app not found. Run "npm run bundle-app".');
    return;
  }

  try {
    const mbzPath = await vs.window.withProgress(
      { location: vs.ProgressLocation.Notification, title: 'Generating course file…' },
      () => generateMbz({ root, dllPath, courseName, date: todayIso() })
    );

    await vs.commands.executeCommand('courseascodetools.refreshExplorer');

    const choice = await vs.window.showInformationMessage(
      `Course file generated: ${path.basename(mbzPath)}`,
      'Reveal in File Explorer'
    );
    if (choice === 'Reveal in File Explorer') {
      await vs.commands.executeCommand('revealFileInOS', vs.Uri.file(mbzPath));
    }
  } catch (err) {
    vs.window.showErrorMessage(err instanceof Error ? err.message : String(err));
  }
}
