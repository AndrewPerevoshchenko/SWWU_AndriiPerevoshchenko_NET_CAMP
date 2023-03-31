namespace Task_1;

internal class Program
{
    static void Main(string[] args)
    {
        Pump pump = new Pump(5);
        User user = new User(20);
        Simulator simulator = new Simulator(12, pump, user);
        simulator.MeetNeeds();
    }
}