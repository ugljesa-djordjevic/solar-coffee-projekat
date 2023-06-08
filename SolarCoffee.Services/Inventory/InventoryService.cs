using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory {
    public class InventoryService : IInventoryService {
        private readonly SolarDbContext _db;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(SolarDbContext dbContext, ILogger<InventoryService> logger) {
            _db = dbContext;
            _logger = logger;
        }
        
        /// <summary>
        /// Vraca sve inventory-e iz baze
        /// </summary>
        /// <returns></returns>
        public List<ProductInventory> GetCurrentInventory() {
            return _db.ProductInventories
                .Include(pi => pi.Product)
                .Where(pi => !pi.Product.IsArchived)
                .ToList();
        }

        /// <summary>
        /// Update-uje broj proizvoda koji su dostupni u inventory-u
        /// QuantityOnHand se moze prilagoditi preko adjustment vrednosti
        /// </summary>
        /// <param name="id">productId</param>
        /// <param name="adjustment">broj dodatih jedinica / brisanje iz inventory-a</param>
        /// <returns></returns>
        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment) {
            var now = DateTime.UtcNow;

            try {
                var inventory = _db.ProductInventories
                    .Include(inv => inv.Product)
                    .First(inv => inv.Product.Id == id);

                inventory.QuantityOnHand += adjustment;

                try {
                    CreateSnapshot();
                }

                catch (Exception e) {
                    _logger.LogError("Error creating inventory snapshot.");
                    _logger.LogError(e.StackTrace);
                }

                _db.SaveChanges();

                return new ServiceResponse<ProductInventory> {
                    IsSuccess = true,
                    Data = inventory,
                    Message = $"Product {id} inventory adjusted",
                    Time = now
                };
            }

            catch {
                return new ServiceResponse<ProductInventory> {
                    IsSuccess = false,
                    Data = null,
                    Message = $"Error updating ProductInventory QuantityOnHand",
                    Time = now
                };
            }
        }

        /// <summary>
        /// Uzima ProductInventory instancu preko indentifikatora Proizvoda
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductInventory GetByProductId(int productId) {
            return _db.ProductInventories
                .Include(pi => pi.Product)
                .FirstOrDefault(pi => pi.Product.Id == productId);
        }

        /// <summary>
        /// Vraca Snapshot za proslih 6 sati
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<ProductInventorySnapshot> GetSnapshotHistory() {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(2);
            
            return _db.ProductInventorySnapshots
                .Include(snap => snap.Product)
                .Where(snap 
                    => snap.SnapshotTime > earliest 
                       && !snap.Product.IsArchived)
                .ToList();
        }
        
        /// <summary>
        /// Pravi Snapshot koristeci ProductInventory instancu
        /// </summary>
        private void CreateSnapshot() {
            var now = DateTime.UtcNow;
            
            var inventories = _db.ProductInventories
                .Include(inv => inv.Product)
                .ToList();

            foreach (var inventory in inventories) {
                var snapshot = new ProductInventorySnapshot {
                    SnapshotTime = now,
                    Product = inventory.Product,
                    QuantityOnHand = inventory.QuantityOnHand
                };

                _db.Add(snapshot);
            }
        }
    }
}