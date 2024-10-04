using Listener;
using System.Net;
using System.Text.Json;

var listener = new HttpListener();
listener.Prefixes.Add(@"http://localhost:27001/");
listener.Start();
var cars = new List<Car>()
{
    new Car{ Id = 1, Model ="Test1" },
    new Car{ Id = 2, Model ="Test2" },

};

while (true)
{

    var context = listener.GetContext();
    var request = context.Request;
    var response = context.Response;
    StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding);
    StreamWriter writer = new StreamWriter(response.OutputStream);
    if (request.HttpMethod == "GET")
    {
        string json = JsonSerializer.Serialize(cars);
        writer.WriteLine(json);
    }
    else if (request.HttpMethod == "DELETE")
    {
        var CarId = int.Parse(reader.ReadToEnd());

        foreach (var car in cars)
        {
            if (car.Id == CarId)
            {
                cars.Remove(car);
                break;
            }

        }
        writer.WriteLine($"removed {CarId}");
    }
    else if (request.HttpMethod == "POST")
    {
        var newCar = reader.ReadToEnd();
        var ID = cars[cars.Count - 1].Id + 1;
        cars.Add(new Car { Id = ID, Model = newCar });
        writer.WriteLine("Added");
    }
    else if (request.HttpMethod == "PUT")
    {
        var newCar = reader.ReadToEnd();

        var car = JsonSerializer.Deserialize<Car>(newCar);  
        if (car is not null)
        {
            foreach (var item in cars)
            {
                if (car.Id == item.Id)
                {
                    item.Model = car.Model;
                    writer.WriteLine("Edited");
                    break;
                }
            }
        }

    }
    writer.Close();
    reader.Close();
}




