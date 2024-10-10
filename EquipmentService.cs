using EquipmentDatabasePopulator5E.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EquipmentDatabasePopulator5E
{
    public class EquipmentService
    {
        private readonly EquipmentContext _context;

        public EquipmentService(EquipmentContext context)
        {
            _context = context;
        }

        public async Task LoadEquipment()
        {
            try
            {
                var equipmentList = new List<Equipment>();
                var equipmentURLs = await LoadURLs("equipment");
                var httpClient = new HttpClient();

                foreach (var url in equipmentURLs)
                {
                    var newEquipment = new Equipment();
                    var response = await httpClient.GetAsync("https://www.dnd5eapi.co" + url);
                    if (response.IsSuccessStatusCode)
                    {
                        //create equipment loader
                        var json = await response.Content.ReadAsStringAsync();
                        var document = JsonDocument.Parse(json);
                        var equipmentLoader = JsonSerializer.Deserialize<EquipmentLoader>(document);

                        //set shared properties
                        Console.WriteLine(equipmentLoader.Name + " parsing");
                        newEquipment = ParseSharedProperties(equipmentLoader, newEquipment);

                        //set category specific properties
                        switch (newEquipment.Category)
                        {
                            case "Armor":
                                newEquipment = ParseArmor(equipmentLoader, newEquipment);
                                break;
                            case "Weapon":
                                newEquipment = ParseWeapon(equipmentLoader, newEquipment);
                                break;
                            case "Adventuring Gear":
                                newEquipment = ParseGear(equipmentLoader, newEquipment);
                                break;
                            case "Tools":
                                newEquipment = ParseTool(equipmentLoader, newEquipment);
                                break;
                            case "Mounts and Vehicles":
                                newEquipment = ParseVehicle(equipmentLoader, newEquipment);
                                break;
                        };

                        //add equipment to list unless parsing failed
                        if (newEquipment != null && !String.IsNullOrWhiteSpace(newEquipment.Name))
                        {
                            equipmentList.Add(newEquipment);
                            Console.WriteLine(newEquipment.Name + " added.");
                        }
                    }
                }

                //insert equipment into database
                if (equipmentList != null && equipmentList.Count > 0)
                {
                    await _context.Equipment.AddRangeAsync(equipmentList);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Equipment added to database.\n");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task LoadEquipmentCategories()
        {
            try
            {
                var categories = new List<EquipmentCategory>();
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://www.dnd5eapi.co/api/equipment-categories");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var document = JsonDocument.Parse(json);
                    var root = document.RootElement.GetProperty("results");
                    categories = JsonSerializer.Deserialize<List<EquipmentCategory>>(root);

                    //insert categories into database
                    if (categories != null)
                    {
                        await _context.Categories.AddRangeAsync(categories);
                        await _context.SaveChangesAsync();
                        Console.WriteLine("\nEquipment Categories added to database.\n");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public async Task LoadMagicEquipment()
        {
            try
            {
                var magicEquipmentList = new List<Equipment>();
                var magicEquipmentURLs = await LoadURLs("magic-items");
                var httpClient = new HttpClient();

                foreach (var url in magicEquipmentURLs)
                {
                    var newEquipment = new Equipment();
                    var response = await httpClient.GetAsync("https://www.dnd5eapi.co" + url);
                    if (response.IsSuccessStatusCode)
                    {
                        //create equipment loader
                        var json = await response.Content.ReadAsStringAsync();
                        var document = JsonDocument.Parse(json);
                        var equipmentLoader = JsonSerializer.Deserialize<EquipmentLoader>(document);

                        //set magic item properties
                        newEquipment = ParseMagicItem(equipmentLoader, newEquipment);

                        //special case for potion of healing. Two items have the same name but must be unique
                        if (newEquipment.Name == "Potion of Healing" && newEquipment.IsVariant == false)
                        {
                            newEquipment.Name = "Potion of Healing (Unclassified)";
                        }

                        //add equipment to list unless parsing failed
                        if (newEquipment != null && !String.IsNullOrWhiteSpace(newEquipment.Name))
                        {
                            magicEquipmentList.Add(newEquipment);
                            Console.WriteLine(newEquipment.Name + " added.");
                        }
                    }
                }

                //insert magic equipment into database
                if (magicEquipmentList != null && magicEquipmentList.Count > 0)
                {
                    await _context.Equipment.AddRangeAsync(magicEquipmentList);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Magic equipment added to database.\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<List<String>> LoadURLs(string category)
        {
            var URLs = new List<String>();

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://www.dnd5eapi.co/api/" + category);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var document = JsonDocument.Parse(json);
                    var root = document.RootElement.GetProperty("results");

                    foreach (var jsonElement in root.EnumerateArray())
                    {
                        var url = ParseStringField(jsonElement, "url");
                        if (!String.IsNullOrWhiteSpace(url))
                        {
                            URLs.Add(url);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return URLs;
        }

        public async Task LoadWeaponProperties()
        {
            try
            {
                var weaponProperties = new List<WeaponProperty>();
                var weaponPropertyURLs = await LoadURLs("weapon-properties");
                var httpClient = new HttpClient();

                foreach (var url in weaponPropertyURLs)
                {
                    var response = await httpClient.GetAsync("https://www.dnd5eapi.co" + url);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var document = JsonDocument.Parse(json);
                        var newWeaponProperty = JsonSerializer.Deserialize<WeaponProperty>(document);

                        if (newWeaponProperty != null)
                        {
                            weaponProperties.Add(newWeaponProperty);
                        }
                    }
                }

                //insert weapon properties into database
                if (weaponProperties != null && weaponProperties.Count > 0)
                {
                    await _context.WeaponProperties.AddRangeAsync(weaponProperties);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Weapon Properties added to database.\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Equipment ParseSharedProperties(EquipmentLoader equipmentLoader, Equipment equipment)
        {
            equipment.Name = equipmentLoader.Name;
            equipment.URL = equipmentLoader.URL;
            equipment.Category = ParseStringField(equipmentLoader.CategoryElement, "name");
            equipment.MagicItem = false;
            equipment.Cost = ConvertToGold(equipmentLoader.CostElement);
            equipment.Weight = equipmentLoader.Weight;

            //description element may contain multiple strings, so they get concatenated
            equipment.Description = string.Join("; ", equipmentLoader.DescriptionElement.EnumerateArray().Select(d => d.GetString()));

            return equipment;
        }

        public Equipment ParseArmor(EquipmentLoader equipmentLoader, Equipment equipment)
        {
            equipment.ArmorCategory = equipmentLoader.ArmorCategory;
            equipment.ArmorClass = ParseIntField(equipmentLoader.ArmorClassElement, "base");
            equipment.DexBonus = ParseBoolField(equipmentLoader.ArmorClassElement, "dex_bonus");
            equipment.MaxDexBonus = ParseIntField(equipmentLoader.ArmorClassElement, "max_bonus");
            equipment.StrengthMinimum = equipmentLoader.StrengthMinimum;
            equipment.StealthDisadvantage = equipmentLoader.StealthDisadvantage;

            return equipment;
        }

        public Equipment ParseGear(EquipmentLoader equipmentLoader, Equipment equipment)
        {
            equipment.GearCategory = ParseStringField(equipmentLoader.GearCategoryElement, "name");

            return equipment;
        }

        public Equipment ParseMagicItem(EquipmentLoader equipmentLoader, Equipment equipment)
        {
            equipment.MagicItem = true;
            equipment.Name = equipmentLoader.Name;
            equipment.URL = equipmentLoader.URL;
            equipment.Category = ParseStringField(equipmentLoader.CategoryElement, "name");
            equipment.Rarity = ParseStringField(equipmentLoader.RarityElement, "name");
            equipment.ImageURL = equipmentLoader.ImageURL;
            equipment.IsVariant = equipmentLoader.IsVariant;

            //description element may contain multiple strings, so they get concatenated
            equipment.Description = string.Join("; ", equipmentLoader.DescriptionElement.EnumerateArray().Select(d => d.GetString()));

            //check if item has any variants
            if (equipmentLoader.VariantsElement.ValueKind == JsonValueKind.Array)
            {
                equipment.HasVariant = equipmentLoader.VariantsElement.EnumerateArray().Any();
            }

            //extract and concatenate all variant names if they exist
            //var variantNames = new List<string>();
            //foreach (var variantElement in equipmentLoader.VariantsElement.EnumerateArray())
            //{
            //    variantNames.Add(ParseStringField(variantElement, "name"));
            //}
            //equipment.Variants = string.Join("; ", variantNames);

            return equipment;
        }

        public Equipment ParseTool(EquipmentLoader equipmentLoader, Equipment equipment)
        {
            equipment.ToolCategory = equipmentLoader.ToolCategory;

            return equipment;
        }

        public Equipment ParseVehicle(EquipmentLoader equipmentLoader, Equipment equipment)
        {
            equipment.VehicleCategory = equipmentLoader.VehicleCategory;

            //combine speed properties into a single string. Why is it seperate in the API??
            var speedAmount = ParseDoubleField(equipmentLoader.SpeedElement, "quantity");
            var speedUnit = ParseStringField(equipmentLoader.SpeedElement, "unit");
            equipment.Speed = speedAmount + " " + speedUnit;

            return equipment;
        }

        public Equipment ParseWeapon(EquipmentLoader equipmentLoader, Equipment equipment)
        {
            equipment.WeaponCategory = equipmentLoader.WeaponCategory;
            equipment.WeaponRange = equipmentLoader.WeaponRange;
            equipment.RangeCategory = equipmentLoader.RangeCategory;
            equipment.RangeNormal = ParseIntField(equipmentLoader.RangeElement, "normal");
            equipment.RangeLong = ParseIntField(equipmentLoader.RangeElement, "long");
            equipment.ThrowRangeNormal = ParseIntField(equipmentLoader.ThrowRangeElement, "normal");
            equipment.ThrowRangeLong = ParseIntField(equipmentLoader.ThrowRangeElement, "long");
            equipment.DamageDice = ParseStringField(equipmentLoader.DamageElement, "damage_dice");
            equipment.TwoHandedDamageDice = ParseStringField(equipmentLoader.TwoHandedElement, "damage_dice");

            //extract damage type from the damage element
            if (equipmentLoader.DamageElement.ValueKind != JsonValueKind.Undefined && equipmentLoader.DamageElement.ValueKind != JsonValueKind.Null)
            {
                if (equipmentLoader.DamageElement.TryGetProperty("damage_type", out JsonElement damageTypeElement))
                {
                    equipment.DamageType = ParseStringField(damageTypeElement, "name");
                }
            }

            //extract two handed damage type from the two handed element
            if (equipmentLoader.TwoHandedElement.ValueKind != JsonValueKind.Undefined && equipmentLoader.TwoHandedElement.ValueKind != JsonValueKind.Null)
            {
                if (equipmentLoader.TwoHandedElement.TryGetProperty("damage_type", out JsonElement twoHandedTypeElement))
                {
                    equipment.TwoHandedDamageType = ParseStringField(twoHandedTypeElement, "name");
                }
            }

            //extract and concatenate all 'special' attribute descriptions if they exist
            equipment.SpecialAttribute = string.Join("; ", equipmentLoader.SpecialAttributeElement.EnumerateArray().Select(s => s.GetString()));

            return equipment;
        }

        public async Task CreateVariantsReferences()
        {
            try
            {
                //var equipmentVariants = new List<EquipmentVariant>();

                //load magic items with variants
                var baseVariants = new List<Equipment>();
                baseVariants = await _context.Equipment.AsNoTracking().Where(e => e.HasVariant).ToListAsync();

                //load magic items that are variants
                var variants = new List<Equipment>();
                variants = await _context.Equipment.Where(e => e.IsVariant).ToListAsync();

                //load item data from the API
                var httpClient = new HttpClient();
                foreach (var baseItem in baseVariants)
                {
                    var response = await httpClient.GetAsync("https://www.dnd5eapi.co" + baseItem.URL);
                    if (response.IsSuccessStatusCode)
                    {
                        //create equipment loader
                        var json = await response.Content.ReadAsStringAsync();
                        var document = JsonDocument.Parse(json);
                        var equipmentLoader = JsonSerializer.Deserialize<EquipmentLoader>(document);

                        //extract all variant names from API data
                        var variantNames = new List<string>();
                        foreach (var variantElement in equipmentLoader.VariantsElement.EnumerateArray())
                        {
                            variantNames.Add(ParseStringField(variantElement, "name"));
                        }

                        //create relationship
                        foreach (var variantName in variantNames)
                        {
                            //var variantID = variants.First(v => v.Name == variantName).Id;
                            //var newEquipmentVariant = new EquipmentVariant()
                            //{
                            //    ParentEquipmentId = baseItem.Id,
                            //    VariantId = variantID
                            //};
                            //equipmentVariants.Add(newEquipmentVariant);
                            var variant = variants.First(v => v.Name == variantName);
                            variant.ParentEquipmentId = baseItem.Id;
                        }
                    }
                }

                //update the db
                //await _context.AddRangeAsync(equipmentVariants);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task CreatePackContentRelationships()
        {
            try
            {
                var packContents = new List<PackContent>();

                //load pack items
                var packItems = new List<Equipment>();
                packItems = await _context.Equipment.AsNoTracking().Where(e => e.GearCategory == "Equipment Packs").ToListAsync();

                //load content items
                var contentItems = new List<Equipment>();
                contentItems = await _context.Equipment.AsNoTracking().Where(e => e.MagicItem == false && e.GearCategory != "Equipment Packs").ToListAsync();


                //load item data from the API
                var httpClient = new HttpClient();
                foreach (var packItem in packItems)
                {
                    var response = await httpClient.GetAsync("https://www.dnd5eapi.co" + packItem.URL);
                    if (response.IsSuccessStatusCode)
                    {
                        //create equipment loader
                        var json = await response.Content.ReadAsStringAsync();
                        var document = JsonDocument.Parse(json);
                        var equipmentLoader = JsonSerializer.Deserialize<EquipmentLoader>(document);

                        //extract all content items from API data
                        foreach (var contentItemElement in equipmentLoader.PackContentsElement.EnumerateArray())
                        {
                            var itemName = "";
                            var quantity = 0;

                            //extract item and quantity information from element
                            if (contentItemElement.TryGetProperty("item", out JsonElement innerItemElement))
                            {
                                itemName = ParseStringField(innerItemElement, "name");
                            }
                            quantity = ParseIntField(contentItemElement, "quantity");

                            //find ID of matching content item
                            var contentID = contentItems.First(i => i.Name == itemName).Id;

                            //create new relationship object and add to list
                            var newPackContent = new PackContent()
                            {
                                PackId = packItem.Id,
                                ContentId = contentID,
                                Amount = quantity
                            };
                            packContents.Add(newPackContent);
                        }
                    }
                }

                //add relationship items to db
                await _context.AddRangeAsync(packContents);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public async Task CreateWeaponPropertyRelationships()
        {
            try
            {
                var weaponPropertyRelationships = new List<EquipmentWeaponProperty>();

                //load weapon items
                var weapons = new List<Equipment>();
                weapons = await _context.Equipment.AsNoTracking().Where(e => e.MagicItem == false && e.Category == "Weapon").ToListAsync();

                //load weapon properties
                var weaponProperties = new List<WeaponProperty>();
                weaponProperties = await _context.WeaponProperties.ToListAsync();


                //load item data from the API
                var httpClient = new HttpClient();
                foreach (var weapon in weapons)
                {
                    var response = await httpClient.GetAsync("https://www.dnd5eapi.co" + weapon.URL);
                    if (response.IsSuccessStatusCode)
                    {
                        //create equipment loader
                        var json = await response.Content.ReadAsStringAsync();
                        var document = JsonDocument.Parse(json);
                        var equipmentLoader = JsonSerializer.Deserialize<EquipmentLoader>(document);

                        //extract all content items from API data
                        foreach (var weaponPropertyElement in equipmentLoader.WeaponPropertiesElement.EnumerateArray())
                        {
                            //extract name of property and find the ID
                            var weaponPropertyName = "";
                            weaponPropertyName = ParseStringField(weaponPropertyElement, "name");
                            var propertyID = weaponProperties.First(p => p.Name == weaponPropertyName).Id;

                            //create relationship object and add to the list
                            var newWeaponPropertyRelationship = new EquipmentWeaponProperty()
                            {
                                EquipmentId = weapon.Id,
                                WeaponPropertyId = propertyID
                            };

                            weaponPropertyRelationships.Add(newWeaponPropertyRelationship);
                        }
                    }
                }

                //add relationship items to db
                await _context.AddRangeAsync(weaponPropertyRelationships);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //////////////////////
        // helper functions //
        //////////////////////

        //converts the cost of an item to it's value in gold. For example, 25 cp = .25 gp
        public double ConvertToGold(JsonElement costElement)
        {
            int amount = ParseIntField(costElement, "quantity");
            string type = ParseStringField(costElement, "unit");

            switch (type)
            {
                case "cp":
                    return amount * .01;
                case "sp":
                    return amount * .1;
                case "ep":
                    return amount * 2;
                case "pp":
                    return amount * 10;
                default:
                    return amount;
            }
        }

        //these functions extract a selected field from jsonElements
        private bool ParseBoolField(JsonElement jsonElement, string propertyName)
        {
            if (jsonElement.TryGetProperty(propertyName, out JsonElement element))
            {
                return element.GetBoolean();
            }
            return false;
        }

        private int ParseIntField(JsonElement jsonElement, string propertyName)
        {
            if (jsonElement.ValueKind == JsonValueKind.Undefined || jsonElement.ValueKind == JsonValueKind.Null)
            {
                return 0;
            }

            if (jsonElement.TryGetProperty(propertyName, out JsonElement element) && element.TryGetInt32(out int value))
            {
                return value;
            }
            return 0;
        }

        private double ParseDoubleField(JsonElement jsonElement, string propertyName)
        {
            if (jsonElement.ValueKind == JsonValueKind.Undefined || jsonElement.ValueKind == JsonValueKind.Null)
            {
                return 0;
            }

            if (jsonElement.TryGetProperty(propertyName, out JsonElement element) && element.TryGetDouble(out double value))
            {
                return value;
            }
            return 0;
        }

        private string ParseStringField(JsonElement jsonElement, string propertyName)
        {
            if (jsonElement.ValueKind == JsonValueKind.Undefined || jsonElement.ValueKind == JsonValueKind.Null)
            {
                return string.Empty;
            }

            if (jsonElement.TryGetProperty(propertyName, out JsonElement element))
            {
                return element.GetString();
            }
            return string.Empty;
        }
    }
}
