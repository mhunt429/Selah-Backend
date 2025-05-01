//Helpful class for local testing and data generation

var x = Guid.CreateVersion7(DateTime.UtcNow);

var y = Guid.CreateVersion7(DateTime.UtcNow);

bool test= x < y;

Console.WriteLine(test);