using Domain;

var timers = new FacadeService().Start();

Console.ReadLine();

timers.ForEach(x => x.Dispose());
