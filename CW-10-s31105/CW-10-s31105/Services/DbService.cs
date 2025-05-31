using System.Runtime.InteropServices.JavaScript;
using CW_10_s31105.Context;
using CW_10_s31105.DTOs;
using CW_10_s31105.Exceptions;
using CW_10_s31105.Models;
using Microsoft.EntityFrameworkCore;

namespace CW_10_s31105.Services;

public interface IDbService
{
    public Task<ICollection<TripGetDTO>> GetTripsDetailsAsync(int page);
    public Task RemoveClientAsync(int clientId);
    public Task AssignClientToTripAsync(string clientPesel, int tripId);
}

public class DbService(MyDbContext data) : IDbService
{
    public async Task<ICollection<TripGetDTO>> GetTripsDetailsAsync(int page)
    {
        int pageSize = 10;
        return await data.Trips.OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize).Select(st => new TripGetDTO()
        {
            IdTrip = st.IdTrip,
            Name = st.Name,
            Description = st.Description,
            DateFrom = st.DateFrom,
            DateTo = st.DateTo,
        }).ToListAsync();
    }

    public async Task RemoveClientAsync(int clientId)
    {
        var clientTrips = await data.ClientsTrips.FirstOrDefaultAsync(ct => ct.IdClient == clientId);
         if (clientTrips is not null) 
         {
             throw new NotFoundException($"Client with id {clientId} has trips assigned to him"); 
         }
         
         var client = await data.Clients.FirstOrDefaultAsync(ct => ct.IdClient == clientId);
         if (client is null) 
         {
             throw new NotFoundException($"Client with id {clientId} not found"); 
         }

        data.Clients.Remove(client);
        await data.SaveChangesAsync();
    }

    public async Task AssignClientToTripAsync(string clientPesel, int tripId)
    {
        var client = await data.Clients.FirstOrDefaultAsync(c => c.Pesel == clientPesel);
        if (client is not null) 
        {
            throw new NotFoundException($"Client with pesel {clientPesel} already exists"); 
        }
        
        var tripAssiged = await data.ClientsTrips.FirstOrDefaultAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == tripId);
        if (tripAssiged is not null) 
        {
            throw new NotFoundException($"Client with pesel {clientPesel} is already assigned to this trip"); 
        }
        
        var trip = await data.Trips.FirstOrDefaultAsync(t => t.IdTrip == tripId);
        if (trip is null) 
        {
            throw new NotFoundException($"Trip with id {tripId} not found"); 
        }

        if (trip.DateFrom < DateTime.Now)
        {
            throw new NotFoundException("Client can't be assigned to trips that had already happened");
        }
         
        DateTime dateFrom = DateTime.Now;
        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = trip.IdTrip,
            RegisteredAt = int.Parse(dateFrom.ToString("yyyyMMdd")),
        };
        
        await data.ClientsTrips.AddAsync(clientTrip);
        await data.SaveChangesAsync();


    }
}