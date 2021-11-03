using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RotationAssignment.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace RotationAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotationController : ControllerBase
    {
        private List<Terminal> _terminalList;
        private List<Cargo> _cargoList;
        private RotationEditor _rotationList;
        private Prospect _prospect;
        private List<TimeStamp> _prospect1;
        private TimeStamp timeStampToAdd;



        public RotationController(List<Cargo> cargoList, RotationEditor rotationList, 
            Prospect prospect, List<TimeStamp> prospect1)
        {
            //adding Terminals
            _terminalList = new List<Terminal>() {
                new Terminal() { Id = "0175d159-f61f-40b9-aaf7-967dbe461349", Name = "FirstTerminal", ATB = DateTime.Now, ATD = DateTime.Now.AddDays(3) },
                new Terminal() { Id = "02ec54f7-8a50-454f-9503-62eeb2f3f4d0", Name = "SecondTerminal", ATB = DateTime.Now.AddDays(4), ATD = DateTime.Now.AddDays(6) },
                new Terminal() { Id = "035d8512-1093-4e40-bc90-d677d059a380", Name = "ThirdTerminal", ATB = DateTime.Now, ATD = DateTime.Now }};

            if(cargoList != null)
                _cargoList = cargoList;
            if (rotationList != null)
                _rotationList = rotationList;
            _prospect = prospect;
            _prospect1 = prospect1;
            


        }
                
        [HttpGet]
        [Route("getcargoes")]
        public IEnumerable<Cargo> GetCargoes()
        {
            return _cargoList;
        }

        [HttpGet]
        [Route("getrotation")]
        public RotationEditor GetRotation()
        {
            return _rotationList;
        }
        

        

        [HttpGet] 
        [Route("getprospects")]
        public List<TimeStamp> GetProspects()
        {
            if(!_prospect1.Any(t => t.Type == TimeTypes.ATA))
                _prospect1.Add(new TimeStamp() {Type = TimeTypes.ATA, Description = "Arrival", Time = DateTime.Now });

            foreach (var terminal in _rotationList.Terminals)
            {
                var currentTerminal = _terminalList.Find(t => t.Id == terminal.TerminalId);
                timeStampToAdd = new TimeStamp()
                {
                    Id = terminal.TerminalId, Time = currentTerminal.ATB,  Type = TimeTypes.ATB,  Description = currentTerminal.Name
                };
                if (!_prospect1.Contains(timeStampToAdd))
                    _prospect1.Add(timeStampToAdd);

                timeStampToAdd = new TimeStamp()
                {
                    Id = terminal.TerminalId, Time = currentTerminal.ATD, Type = TimeTypes.ATD, Description = currentTerminal.Name
                };
                if (!_prospect1.Contains(timeStampToAdd))
                    _prospect1.Add(timeStampToAdd);

            }
            _prospect1.Sort();
            return _prospect1;
        }

        [HttpPost]
        [Route("createcargo")]
        public IActionResult CreateCargo(Cargo cargo)
        {
            if (!ValidateIfCargoAlreadyUsed(cargo.Id))
            {
                _cargoList.Add(cargo);
                AddCargoToRotation(cargo);
                timeStampToAdd = new TimeStamp() { Time = cargo.ATC, Type = TimeTypes.ATC, Description = cargo.Name, Id = cargo.Id };
                if (!_prospect1.Contains(timeStampToAdd))
                    _prospect1.Add(timeStampToAdd);
                return new StatusCodeResult((int)HttpStatusCode.Created);

            }

            return BadRequest(string.Format("Cargo id = {0} is already used in the rotation", cargo.Id));
        }

        
        [HttpDelete("{id}")]
        public StatusCodeResult DeleteCargo(string id)
        {
            int cargoToDeleteIndex = _cargoList.FindIndex(c => c.Id == id);
            if (cargoToDeleteIndex > 0)
            {
                DeleteCargoFromRotation(_cargoList[cargoToDeleteIndex]);
                _cargoList.RemoveAt(cargoToDeleteIndex);
                _prospect1.RemoveAt(_prospect1.FindIndex(s => s.Id == id));

            }
            else
            {
                return NotFound();
        
            }
        
            return NoContent();
        }
        

        [HttpPut]
        public IActionResult EditRotation(RotationEditor rotationChanges)
        {
            //if terminal used more then once
            if(ValidateIfUsedMoreThenOnce(rotationChanges.Terminals))
                return BadRequest("Some terminals used more then once");

            List<RotationEditor.Cargo> cargoValidationList = new List<RotationEditor.Cargo>();
            foreach (var terminal in rotationChanges.Terminals)
            {
                //if terminal has bad id
                if (!_terminalList.Any(t => t.Id == terminal.TerminalId))
                    return BadRequest(string.Format("bad terminal id = {0}", terminal.TerminalId));

                foreach (var cargo in terminal.Cargoes)
                {
                    //if cargo has bad id
                    if (!_cargoList.Any(c => c.Id == cargo.CargoId))
                        return BadRequest(string.Format("bad cargo id = {0}", cargo.CargoId));

                    cargoValidationList.Add(cargo);
                }
            }
            //if cargo used more then once
            if (ValidateIfUsedMoreThenOnce(cargoValidationList))
                return BadRequest("Some cargoes used more then once");

            //rotation editing
            //List<RotationEditor.Cargo> cargoesToRemove = new List<RotationEditor.Cargo>();
            foreach (var rcterminal in rotationChanges.Terminals)
            {
                var currentTerminalCargoes = _rotationList.Terminals.Find(c => c.TerminalId == rcterminal.TerminalId).Cargoes;
                currentTerminalCargoes.Clear();
                foreach (var cargo in rcterminal.Cargoes)
                {
                    //removing cargoes from previous places
                    var oldTerminalId = _cargoList.Find(c => c.Id == cargo.CargoId).TerminalId;
                    var oldTerminalInCargoList = _rotationList.Terminals.Find(t => t.TerminalId == oldTerminalId);
                    var cargoObject = oldTerminalInCargoList.Cargoes.Find(c => c.CargoId == cargo.CargoId);
                    oldTerminalInCargoList.Cargoes.Remove(cargoObject);
                    _prospect1.Remove(_prospect1.Find(s => s.Id == cargo.CargoId));
                    //Remove terminal if it's cargo list is empty
                    if (oldTerminalInCargoList.Cargoes.Count() == 0)
                    {
                        _rotationList.Terminals.Remove(oldTerminalInCargoList);
                        _prospect1.RemoveAll(s => s.Id == oldTerminalId);
                    }
                        

                    //adding cargoes to new places
                    currentTerminalCargoes.Add(new RotationEditor.Cargo() { CargoId = cargo.CargoId });
                    oldTerminalId = rcterminal.TerminalId;
                   // _prospect1.Add(new TimeStamp() { Id = cargo.CargoId, })/////////////////////////
                    //cargoesToRemove.Add(cargo);
                } 
                                
            }         
            
            return Ok();
        }
            
        private bool ValidateIfCargoAlreadyUsed(string id)
        {
            return _cargoList.Any(c => c.Id == id);
        }
        private bool ValidateIfUsedMoreThenOnce<T>(List<T> obj)
        {
            return !(obj.Count() == obj.Distinct().Count());
        }
        private void AddCargoToRotation(Cargo cargo)
        {
            //if terminal does not exist, then create
            if (!_rotationList.Terminals.Any(c => c.TerminalId == cargo.TerminalId))
            {
                _rotationList.Terminals.Add(new RotationEditor.Terminal() { TerminalId = cargo.TerminalId });
            }

            _rotationList.Terminals.Find(c => c.TerminalId == cargo.TerminalId).Cargoes.Add(new RotationEditor.Cargo() { CargoId = cargo.Id });

        }
        private void DeleteCargoFromRotation(Cargo cargo)
        {
            if (_rotationList.Terminals.Any(c => c.TerminalId == cargo.TerminalId))
            {
                _rotationList.Terminals.Find(c => c.TerminalId == cargo.TerminalId).Cargoes.RemoveAll(c => c.CargoId == cargo.Id);
            }
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
