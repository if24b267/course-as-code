import java.util.Scanner;


public class Main {
	public static void main(String[] args) {

		Scanner sc = new Scanner(System.in); 
		System.out.print(": ");
		int n = sc.nextInt();
		while(n>1){
			System.out.printf("%d\n", n);
			n = (n%2==0)?n/2:3*n+1;
		}
		System.out.printf("%d\n", n);
		
		
		sc.close();
		
		
	}
}