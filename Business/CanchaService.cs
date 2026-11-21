using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testing.Data;
using Testing.Entities;


namespace Testing.Business;

public class CanchaService
{
    private readonly CanchaRepository _repository;

    public CanchaService(CanchaRepository repository)
    {
        _repository = repository;
    }
}