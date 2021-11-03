using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RotationAssignment.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RotationAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotationController : ControllerBase
    {
        private List<Terminal> terminalList;
        private List<Cargo> _cargoList;
        private OrderedDictionary _rotationList;
        

        public RotationController(List<Cargo> cargoList, OrderedDictionary rotationList)
        {
            //adding terminals
            terminalList = new List<Terminal>() {
                new Terminal() { Id = "0175d159-f61f-40b9-aaf7-967dbe461349", Name = "FirstTerminal", ATB = DateTime.Now, ATD = DateTime.Now },
                new Terminal() { Id = "02ec54f7-8a50-454f-9503-62eeb2f3f4d0", Name = "SecondTerminal", ATB = DateTime.Now, ATD = DateTime.Now },
                new Terminal() { Id = "035d8512-1093-4e40-bc90-d677d059a380", Name = "ThirdTerminal", ATB = DateTime.Now, ATD = DateTime.Now }};

            if(cargoList != null)
                _cargoList = cargoList;
            if (rotationList != null)
                _rotationList = rotationList;
                       
        }
                
        [HttpGet]
        [Route("getcargoes")]
        public IEnumerable<Cargo> GetCargoes()
        {
            return _cargoList;
        }

        [HttpGet]
        [Route("getrotation")]
        public OrderedDictionary GetRotation()
        {
            return _rotationList;
        }

        [HttpPost]
        [Route("createcargo")]
        public StatusCodeResult CreateCargo(Cargo cargo)
        {
            if (!_cargoList.Any(c => c.Id == cargo.Id))
            {
                _cargoList.Add(cargo);
                AddCargoToRotation(cargo);
                return new StatusCodeResult((int)HttpStatusCode.Created);

            }
        
            return new StatusCodeResult((int)HttpStatusCode.Conflict);
        }

        
        [HttpDelete("{id}")]
        public StatusCodeResult DeleteCargo(string id)
        {
            int cargoToDeleteIndex = _cargoList.FindIndex(c => c.Id == id);
            if (cargoToDeleteIndex > 0)
            {
                DeleteCargoFromRotation(_cargoList[cargoToDeleteIndex]);
                _cargoList.RemoveAt(cargoToDeleteIndex);
                
        
            }
            else
            {
                return NotFound();
        
            }
        
            return NoContent();
        }
        private void AddCargoToRotation(Cargo cargo)
        {
            if (!_rotationList.Contains(cargo.TerminalId))
            {
                _rotationList.Add(cargo.TerminalId, new List<string>());
            }
            (_rotationList[cargo.TerminalId] as List<string>).Add(cargo.Id);
        }
        private void DeleteCargoFromRotation(Cargo cargo)
        {
            if (_rotationList.Contains(cargo.TerminalId))
            {
                _rotationList.Remove(cargo.TerminalId);
            }
        }

        [HttpPut]
        public StatusCodeResult EditRotation(RotationEditor rotationChanges)
        {
            foreach(var terminal in rotationChanges.terminals)
            {
                if (_rotationList.Contains(terminal.TerminalId))
                {
                    foreach (var cargo in terminal.Cargoes)
                    {
                        (_rotationList[terminal.TerminalId] as List<string>).Add(cargo.CargoId);
                    }
                }
                else
                {
                    _rotationList.Add(terminal.TerminalId, terminal.Cargoes);
                }
                
                                
            }
            return BadRequest();


        }
        // GET: api/TodoItems/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Cargo>> GetCargo(string id)
        //{
        //    var cargo = await _context.Cargoes.FindAsync(id);
        //
        //    if (cargo == null)
        //    {
        //        return NotFound();
        //    }
        //
        //    return cargo; 
        //}
        //
        //
        //[Route("getrotation")]
        //public async Task<ActionResult<IEnumerable<Terminal>>> GetRotation()
        //{
        //    return await _context.Terminals.ToListAsync();
        //}
        //
        //// POST: api/TodoItems
        //[HttpPost]
        //public async Task<ActionResult<Cargo>> PostCargo(Cargo cargo)
        //{
        //    if (CargoExists(cargo.Id))
        //        return BadRequest("Cargo already used in this rotation");
        //    _context.Cargoes.Add(cargo);
        //    await _context.SaveChangesAsync();
        //
        //    //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        //    return CreatedAtAction(nameof(GetCargo), new { id = cargo.Id }, cargo);
        //}
        //
        //[Route("postcargolist")]
        //public async Task<ActionResult<Cargo>> PostCargoList(List<Cargo> cargo)
        //{
        //    _context.Cargoes.AddRange(cargo);
        //    await _context.SaveChangesAsync();
        //
        //    //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        //    return CreatedAtAction(nameof(GetCargo), new { id = cargo.Last().Id }, cargo); ;
        //}
        //
        //// PUT: api/TodoItems/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTodoItem(string id, Cargo cargo)
        //{
        //    if (id != cargo.Id)
        //    {
        //        return BadRequest();
        //    }
        //
        //    _context.Entry(cargo).State = EntityState.Modified;
        //
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CargoExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //
        //    return NoContent();
        //}
        //private bool CargoExists(string id)
        //{
        //    return _context.Cargoes.Any(c => c.Id == id);
        //}
    }
}
