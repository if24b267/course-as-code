import java.util.Scanner;

public class Main {
	/**
	 * Recursive function to solve the towers of hanoi problem.<br>
	 * 
	 * This function produces textual output of the necessary moves to solve the
	 * tower of hanoi problem for any number of discs.<br>
	 * It moves a disc of specified size n from a specified from_rod to another
	 * specified to_rod (and uses another specified temp_rod as temporary storage).
	 * It does this by
	 * 
	 * <ul>
	 * <li>(recursively) moving all smaller discs (size&lt;n) to the temporary
	 * rod</li>
	 * <li>moving disc of size n to to_rod</li>
	 * <li>(recursively) moving all smaller discs (size&lt;n) from the temporary rod
	 * to the to_rod</li>
	 * </ul>
	 * 
	 * 
	 * @param n
	 *            the size of the disk we want to move
	 * @param from_rod
	 *            the rod where the disk is before the move
	 * @param to_rod
	 *            the rod where the disk should be moved to
	 * @param temp_rod
	 *            the rod where other disks than the one we are moving can be
	 *            temporarily stored
	 */
	static void move(int n, String from_rod, String to_rod, String temp_rod) {

		if (n == 1) {
			System.out.println("--> Moving disk 1 from rod " + from_rod + " to rod " + to_rod);
			return;
		}
		move(n - 1, from_rod, temp_rod, to_rod);
		System.out.println("--> Moving disk " + n + " from rod " + from_rod + " to rod " + to_rod);
		move(n - 1, temp_rod, to_rod, from_rod);

	}


	public static void demo() {
		Scanner sc = new Scanner(System.in);
		int n = sc.nextInt(); // Number of disks
		move(n, "A", "C", "B"); // A, B and C are names of rods
		sc.close();
	}

	public static void main(String args[]) {
		Scanner sc = new Scanner(System.in);
		System.out.printf("discs: ");
		int n = sc.nextInt(); 
		System.out.println();
		System.out.printf("%d move(s) in total.\n", towersOfHanoi(n, "A", "C", "B"));
		sc.close();
	}
}