using DotMemoryExplorer.DebugeeConsole;

SecretData? secret = null;

void PrintObjects() {
	SecretData? current = secret;
	while (current != null) {
		Console.WriteLine($"Secret item {current.Text} with number {current.Number}");
		current = current.Prev;
	}
}

void AddObject() {
	Console.WriteLine("Enter secrete text: ");
	string text = Console.ReadLine() ?? "";
	Console.WriteLine("Enter secrete number: ");
	int num;
	try {
		num = int.Parse(Console.ReadLine() ?? "");
	} catch (Exception) {
		Console.WriteLine("Unable to parse. Ignoring.");
		return;
	}

	secret = new SecretData(text, num, secret);
}

while (true) {
	Console.WriteLine("==================");
	Console.WriteLine("Enter command ((p)rint/(a)dd/(c)lose):");

	char command = Console.ReadKey().KeyChar;

	Console.WriteLine();
	
	if (command == 'p') {
		PrintObjects();
	} else if (command == 'a') {
		AddObject();
	} else if (command == 'c') {
		return;
	} else {
		Console.WriteLine("Unknown command");
	}
}
