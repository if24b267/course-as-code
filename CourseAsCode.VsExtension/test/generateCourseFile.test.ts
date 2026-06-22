import * as assert from 'assert';
import * as fs from 'fs';
import * as os from 'os';
import * as path from 'path';
import {
  readCourseName,
  findSingleMbz,
  generateMbz,
  Spawner,
} from '../src/commands/generateCourseFile';

function makeTempDir(): string {
  return fs.mkdtempSync(path.join(os.tmpdir(), 'cac-gen-'));
}

describe('readCourseName', () => {
  it('returns undefined when config.yaml is missing', () => {
    const root = makeTempDir();
    assert.strictEqual(readCourseName(root), undefined);
    fs.rmSync(root, { recursive: true });
  });

  it('returns CourseName from config.yaml', () => {
    const root = makeTempDir();
    fs.writeFileSync(path.join(root, 'config.yaml'), 'CourseName: My Course\nShortName: MC\n');
    assert.strictEqual(readCourseName(root), 'My Course');
    fs.rmSync(root, { recursive: true });
  });
});

describe('findSingleMbz', () => {
  it('returns the single .mbz path', () => {
    const dir = makeTempDir();
    fs.writeFileSync(path.join(dir, 'out.mbz'), 'x');
    assert.strictEqual(findSingleMbz(dir), path.join(dir, 'out.mbz'));
    fs.rmSync(dir, { recursive: true });
  });

  it('throws when no .mbz is present', () => {
    const dir = makeTempDir();
    assert.throws(() => findSingleMbz(dir), /No \.mbz/);
    fs.rmSync(dir, { recursive: true });
  });

  it('throws when more than one .mbz is present', () => {
    const dir = makeTempDir();
    fs.writeFileSync(path.join(dir, 'a.mbz'), 'x');
    fs.writeFileSync(path.join(dir, 'b.mbz'), 'x');
    assert.throws(() => findSingleMbz(dir), /Expected one/);
    fs.rmSync(dir, { recursive: true });
  });
});

describe('generateMbz', () => {
  // Fake spawner that writes a .mbz into the staging dir passed via -o.
  const okSpawn: Spawner = async (_cmd, args) => {
    const stagingDir = args[args.indexOf('-o') + 1];
    fs.writeFileSync(path.join(stagingDir, 'course.mbz'), 'data');
    return { code: 0, stdout: 'DONE!', stderr: '' };
  };

  it('copies the produced mbz into output/ with the versioned name', async () => {
    const root = makeTempDir();
    const target = await generateMbz({
      root, dllPath: 'dummy.dll', courseName: 'My Course', date: '2026-06-15', spawn: okSpawn,
    });
    assert.strictEqual(path.basename(target), 'My_Course_2026-06-15.mbz');
    assert.ok(fs.existsSync(target));
    fs.rmSync(root, { recursive: true });
  });

  it('versions the name when a same-day release exists', async () => {
    const root = makeTempDir();
    fs.mkdirSync(path.join(root, 'output'));
    fs.writeFileSync(path.join(root, 'output', 'My_Course_2026-06-15.mbz'), 'old');
    const target = await generateMbz({
      root, dllPath: 'dummy.dll', courseName: 'My Course', date: '2026-06-15', spawn: okSpawn,
    });
    assert.strictEqual(path.basename(target), 'My_Course_2026-06-15_2.mbz');
    fs.rmSync(root, { recursive: true });
  });

  it('throws a friendly error when dotnet is missing (ENOENT)', async () => {
    const root = makeTempDir();
    const enoent: Spawner = async () => {
      const e = new Error('spawn dotnet ENOENT') as NodeJS.ErrnoException;
      e.code = 'ENOENT';
      throw e;
    };
    await assert.rejects(
      () => generateMbz({ root, dllPath: 'd', courseName: 'C', date: '2026-06-15', spawn: enoent }),
      /\.NET 8 runtime is required/
    );
    fs.rmSync(root, { recursive: true });
  });

  it('throws with output tail when the app exits non-zero', async () => {
    const root = makeTempDir();
    const fail: Spawner = async () => ({ code: 1, stdout: 'Error: Writing course failed.', stderr: '' });
    await assert.rejects(
      () => generateMbz({ root, dllPath: 'd', courseName: 'C', date: '2026-06-15', spawn: fail }),
      /Writing course failed/
    );
    fs.rmSync(root, { recursive: true });
  });

  it('throws when no .mbz was produced', async () => {
    const root = makeTempDir();
    const noMbz: Spawner = async () => ({ code: 0, stdout: 'DONE!', stderr: '' });
    await assert.rejects(
      () => generateMbz({ root, dllPath: 'd', courseName: 'C', date: '2026-06-15', spawn: noMbz }),
      /No \.mbz/
    );
    fs.rmSync(root, { recursive: true });
  });
});
