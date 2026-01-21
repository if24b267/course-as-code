import java.util.Scanner;

public class Main {

	public static void main(String[] args) {

		Scanner sc = new Scanner(System.in);
		System.out.printf(": ");
		int sizX = /* sc.nextInt(); */ 32;
		int sizY = /* sc.nextInt(); */ 32;

		int[][] cField = new int[sizX][sizY];

		int x = 0;

		while (true) {
			if ((x = sc.nextInt()) < 0)
				break;
			cField[x][sc.nextInt()] = 1;
		}

		/*
		 * cField[10][10] = 1; cField[10][11] = 1; cField[10][12] = 1;
		 * 
		 * cField[0][10] = 1; cField[0][11] = 1; cField[0][12] = 1;
		 * 
		 * 
		 * cField[15][15] = 1; cField[15][16] = 1; cField[16][15] = 1; cField[16][16] =
		 * 1;
		 */

		/*
		 * for (int i = 0; i < sizX; i++) for (int j = 0; j < sizY; j++) if
		 * (Math.random() > 0.80) { cField[i][j] = 1; System.out.printf("%d %d\n", i,
		 * j); }
		 */

		int target = 0;

		for (int i = 0; true; i++) {
			if (i == target) {
				print(cField);
				System.out.printf("(%06d): ", i);
				int inc = sc.nextInt();
				if (inc <= 0)
					break;
				target += inc;

			}

			cField = step(cField);
		}

		print(cField);
		sc.close();

	}

	public static void print(int[][] f) {
		System.out.printf("\n");
		for (int i = 0; i < f.length; i++) {
			for (int j = 0; j < f[0].length; j++) {
				System.out.printf("%c", f[i][j] == 0 ? '.' : 'X');
			}
			System.out.printf("\n");
		}

	}

	private static int[][] step(int[][] cField) {
		int[][] nField = new int[cField.length][cField[0].length];
		for (int i = 0; i < nField.length; i++) {
			for (int j = 0; j < nField[0].length; j++) {
				if (cField[i][j] == 1 && nAlive(cField, i, j) < 2) {
					nField[i][j] = 0;
				} else if (cField[i][j] == 1 && nAlive(cField, i, j) < 4) {
					nField[i][j] = 1;
				} else if (cField[i][j] == 1 && nAlive(cField, i, j) > 3) {
					nField[i][j] = 0;
				} else if (cField[i][j] == 0 && nAlive(cField, i, j) == 3) {
					nField[i][j] = 1;
				}

			}
		}

		return nField;
	}

	public static int nAlive(int[][] cField, int i, int j) {

		// relative coordinates of 8-connected neighbours
		int[][] idx = { //
				{ -1, -1 }, // NW
				{ -1, 0 }, // N
				{ -1, 1 }, // NE
				{ 0, -1 }, // W
				{ 0, 1 }, // E
				{ 1, -1 }, // SW
				{ 1, 0 }, // S
				{ 1, 1 }, // SE
		};

		// we assume all neighbors exist and select them all
		int[] sel = { 1, 1, 1, 1, 1, 1, 1, 1 };

		if (i == 0) { // northern edge
			sel[0] = sel[1] = sel[2] = 0; // unselect NW, N, NE
		}
		if (j == 0) { // western edge
			sel[0] = sel[3] = sel[5] = 0; // unselect NW, W, SW
		}
		if (i == cField.length - 1) { // southern edge
			sel[5] = sel[6] = sel[7] = 0; // unselect SW, S, SW
		}
		if (j == cField[0].length - 1) { // eastern edge
			sel[2] = sel[4] = sel[7] = 0; // unselect NE, E, SE
		}

		int cnt = 0;
		for (int a = 0; a < sel.length; a++)
			if (sel[a] == 1) {
				cnt += cField[i + idx[a][0]][j + idx[a][1]];
			}
		// System.out.printf("%d\n", cnt);
		return cnt;
	}
}