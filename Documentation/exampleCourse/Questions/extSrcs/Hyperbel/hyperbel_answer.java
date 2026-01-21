import java.util.Scanner;

public class Main {
	public static void main(String[] args) {

		Scanner sc = new Scanner(System.in); 
		
	    int a = 0;
	    int b = 0;
	    int c = 0;
	    int d = 0;
	    double e = 0;

	    System.out.printf("a: ");
	    a=sc.nextInt();
	    System.out.printf("b: ");
	    b=sc.nextInt();
	    System.out.printf("c: ");
	    c=sc.nextInt();
	    System.out.printf("d: ");

	    d=sc.nextInt();
	    System.out.printf("e: ");
	    e=sc.nextDouble();
	    
	    System.out.printf("\n01234567890123456\n");
	    System.out.printf("\\               /\n");
	    System.out.printf(" \\ %011d /\n", a);
	    System.out.printf("  \\           /\n");
	    System.out.printf("%-3d| %07.3f |%3d\n", b, e, c);
	    System.out.printf("  /           \\\n");
	    System.out.printf(" / %011d \\\n", d);
	    System.out.printf("/               \\\n");
		sc.close();
		
		
	}
}