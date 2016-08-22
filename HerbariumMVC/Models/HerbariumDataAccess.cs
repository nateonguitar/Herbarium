using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.IO;

namespace HerbariumMVC.Models
{
    public class HerbariumDataAccess
    {

        // ***** methods for accessing the MSAccess database
        public List<SearchResultViewModel> getAllSpecimensWithAcquisitionAndLocationData(bool multipleSearch)
        {
            List<SearchResultViewModel> searchResultViewModels = new List<SearchResultViewModel>();
            DataSet ds = new DataSet();
            OleDbConnection conn = new OleDbConnection();

            bool querySuccessful = false;

            String sqlStatement = "SELECT "
                + "[Specimen].[SpecimenGUID], "
                + "[Specimen].[Family], "
                + "[Specimen].[Genus], "
                + "[Specimen].[Species], "
                + "[Specimen].[SubSpecies], "
                + "[Specimen].[Author], "
                + "[Specimen].[SubAuthor], "
                + "[Specimen].[Community], "
                + "[Specimen].[SoilType], "
                + "[Specimen].[SoilColor], "
                + "[Specimen].[SoilMoisture], "
                + "[Specimen].[SIKey], "
                + "[Acquisition].[AcquisitionNo], "
                + "[Acquisition].[EthnobotanicalUse], "
                + "[Acquisition].[CollectedDate], "
                + "[Acquisition].[CollectorNo], "
                + "[Acquisition].[Notes], "
                + "[Acquisition].[CollectorName], "
                + "[Acquisition].[Deleted], "
                + "[Acquisition].[AIKey], "
                + "[Acquisition].[UserName], "
                + "[Location].[LocationGUID], "
                + "[Location].[Lat], "
                + "[Location].[Lng], "
                + "[Location].[Elevation], "
                + "[Location].[Country], "
                + "[Location].[County], "
                + "[Location].[State], "
                + "[Location].[LocationDesc], "
                + "[Location].[LIKey], "
                + "[Location].[Township], "
                + "[Location].[Sect], "
                + "[Location].[Range], "
                + "[Image].[IIKey], "
                + "[Image].[ImageName] "

                + "FROM (([Location] "
                + "INNER JOIN [Specimen]    ON [Location].[LocationGUID] = [Specimen].[LocationGUID]) "
                + "INNER JOIN [Acquisition] ON [Specimen].[SpecimenGUID] = [Acquisition].[SpecimenGUID]) "
                + "INNER JOIN [Image]       ON [Location].[LocationGUID] = [Image].[LocationGUID] "

                + "ORDER BY [Specimen].[SpecimenGUID];";
            
            
            try
            {
                conn.ConnectionString = connStr();
                OleDbCommand cmd = new OleDbCommand(sqlStatement, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                searchResultViewModels = convertSearchResultViewModels(ds); // OleDb returns a dataset with a datatable - convert to a class

                if (!multipleSearch)
                {
                    // remove duplicates from the List<>()
                    // they should already be ordered by [Specimen].[SpecimenGUID]
                    for (int i = (searchResultViewModels.Count() - 1); i > 0; i--)
                    {
                        if (searchResultViewModels.ElementAt(i).SpecimenGUID.Equals(searchResultViewModels.ElementAt(i - 1).SpecimenGUID))
                        {
                            searchResultViewModels.RemoveAt(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return searchResultViewModels;
        }


        public bool updateSpecimen(SearchResultViewModel entry, string imageNameWithoutPath)
        {
            SearchResultViewModel specimen = entry;

            bool updateSuccessful = false;

            // trim off white space from left and right of each data member
            specimen.EthnobotanicalUse = specimen.EthnobotanicalUse?.Trim();
            specimen.Notes             = specimen.Notes            ?.Trim();
            specimen.CollectorName     = specimen.CollectorName    ?.Trim();
            specimen.CollectorNo       = specimen.CollectorNo      ?.Trim();
            specimen.Country           = specimen.Country          ?.Trim();
            specimen.County            = specimen.County           ?.Trim();
            specimen.State             = specimen.State            ?.Trim();
            specimen.LocationDesc      = specimen.LocationDesc     ?.Trim();
            specimen.Township          = specimen.Township         ?.Trim();
            specimen.Family            = specimen.Family           ?.Trim();
            specimen.Genus             = specimen.Genus            ?.Trim();
            specimen.Species           = specimen.Species          ?.Trim();
            specimen.SubSpecies        = specimen.SubSpecies       ?.Trim();
            specimen.Author            = specimen.Author           ?.Trim();
            specimen.SubAuthor         = specimen.SubAuthor        ?.Trim();
            specimen.Community         = specimen.Community        ?.Trim();
            specimen.SoilType          = specimen.SoilType         ?.Trim();
            specimen.SoilColor         = specimen.SoilColor        ?.Trim();
            specimen.Range             = specimen.Range            ?.Trim();
            specimen.Sect              = specimen.Sect             ?.Trim();
            specimen.UserName          = specimen.UserName         ?.Trim();
            specimen.ImageName         = specimen.ImageName        ?.Trim();


            if (String.IsNullOrEmpty(specimen.EthnobotanicalUse))
            {
                specimen.EthnobotanicalUse = " ";
            }
            if (String.IsNullOrEmpty(specimen.Notes))
            {
                specimen.Notes = " ";
            }
            if (String.IsNullOrEmpty(specimen.CollectorName))
            {
                specimen.CollectorName = " ";
            }

            if (String.IsNullOrEmpty(specimen.CollectorNo))
            {
                specimen.CollectorNo = " ";
            }
            if (String.IsNullOrEmpty(specimen.Country))
            {
                specimen.Country = " ";
            }
            if (String.IsNullOrEmpty(specimen.County))
            {
                specimen.County = " ";
            }
            if (String.IsNullOrEmpty(specimen.State))
            {
                specimen.State = " ";
            }
            if (String.IsNullOrEmpty(specimen.LocationDesc))
            {
                specimen.LocationDesc = " ";
            }
            if (String.IsNullOrEmpty(specimen.Township))
            {
                specimen.Township = " ";
            }
            if (String.IsNullOrEmpty(specimen.Family))
            {
                specimen.Family = " ";
            }
            if (String.IsNullOrEmpty(specimen.Genus))
            {
                specimen.Genus = " ";
            }
            if (String.IsNullOrEmpty(specimen.Species))
            {
                specimen.Species = " ";
            }
            if (String.IsNullOrEmpty(specimen.SubSpecies))
            {
                specimen.SubSpecies = " ";
            }
            if (String.IsNullOrEmpty(specimen.Author))
            {
                specimen.Author = " ";
            }
            if (String.IsNullOrEmpty(specimen.SubAuthor))
            {
                specimen.SubAuthor = " ";
            }
            if (String.IsNullOrEmpty(specimen.Community))
            {
                specimen.Community = " ";
            }
            if (String.IsNullOrEmpty(specimen.SoilType))
            {
                specimen.SoilType = " ";
            }
            if (String.IsNullOrEmpty(specimen.SoilColor))
            {
                specimen.SoilColor = " ";
            }
            if (String.IsNullOrEmpty(specimen.SoilMoisture))
            {
                specimen.SoilMoisture = " ";
            }
            if (String.IsNullOrEmpty(specimen.Range))
            {
                specimen.Range = " ";
            }
            if (String.IsNullOrEmpty(specimen.Sect))
            {
                specimen.Sect = " ";
            }
            if (String.IsNullOrEmpty(specimen.UserName))
            {
                specimen.UserName = " ";
            }
            if (String.IsNullOrEmpty(specimen.ImageName))
            {
                specimen.ImageName = " ";
            }



            OleDbConnection conn = new OleDbConnection();
            String sqlStatement = "UPDATE  [Acquisition] "
                                    + "SET AcquisitionNo = @AcquisitionNo, "
                                    + "EthnobotanicalUse = @EthnobotanicalUse, "
                                    + "CollectedDate = @CollectedDate, "
                                    + "CollectorNo = @CollectorNo, "
                                    + "Notes = @Notes, "
                                    + "CollectorName = @CollectorName "
                                    + "WHERE [AIKey] = @AIKey;";
            try
            {
                conn.ConnectionString = connStr();
                OleDbCommand cmd = new OleDbCommand(sqlStatement, conn);
                //parameters are taken in order - not by name
                cmd.Parameters.AddWithValue("@AcquisitionNo", specimen.AcquisitionNo);
                cmd.Parameters.AddWithValue("@EthnobotanicalUse", specimen.EthnobotanicalUse);
                cmd.Parameters.AddWithValue("@CollectedDate", specimen.CollectedDate);
                cmd.Parameters.AddWithValue("@CollectorNo", specimen.CollectorNo);
                cmd.Parameters.AddWithValue("@Notes", specimen.Notes);
                cmd.Parameters.AddWithValue("@CollectorName", specimen.CollectorName);
                cmd.Parameters.AddWithValue("@AIKey", specimen.AIKey);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                // if it didn't work it wouldn't not get this far
                // lets update the next table

                conn = new OleDbConnection();
                sqlStatement = "UPDATE  [Location] "
                                + "SET Lat = @Lat, "
                                + "Lng = @Lng, "
                                + "Elevation = @Elevation, "
                                + "Country = @Country, "
                                + "County = @County, "
                                + "State = @State, " 
                                + "LocationDesc = @LocationDesc, "
                                + "Township = @Township "
                                + "WHERE [LIKey] = @LIKey;";
                try
                {
                    conn.ConnectionString = connStr();
                    cmd = new OleDbCommand(sqlStatement, conn);
                    //parameters are taken in order - not by name
                    cmd.Parameters.AddWithValue("@Lat", specimen.Lat);
                    cmd.Parameters.AddWithValue("@Lng", specimen.Lng);
                    cmd.Parameters.AddWithValue("@Elevation", specimen.Elevation.ToString());
                    cmd.Parameters.AddWithValue("@Country", specimen.Country);
                    cmd.Parameters.AddWithValue("@County", specimen.County);
                    cmd.Parameters.AddWithValue("@State", specimen.State);
                    cmd.Parameters.AddWithValue("@LocationDesc", specimen.LocationDesc);
                    cmd.Parameters.AddWithValue("@Township", specimen.Township);
                    cmd.Parameters.AddWithValue("@LIKey", specimen.LIKey);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    // if it didn't work it wouldn't not get this far
                    // lets update the next table

                    conn = new OleDbConnection();
                    sqlStatement = "UPDATE  [Specimen] "
                                    + "SET Family = @Family, "
                                    + "Genus = @Genus, "
                                    + "Species = @Species, "
                                    + "SubSpecies = @SubSpecies, "
                                    + "Author = @Author, "
                                    + "SubAuthor = @SubAuthor, "
                                    + "Community = @Community, "
                                    + "SoilType = @SoilType, "
                                    + "SoilColor = @SoilColor, "
                                    + "SoilMoisture = @SoilMoisture "
                                    + "WHERE [SIKey] = @SIkey;";
                    try
                    {
                        conn.ConnectionString = connStr();
                        cmd = new OleDbCommand(sqlStatement, conn);
                        //parameters are taken in order - not by name
                        cmd.Parameters.AddWithValue("@Family", specimen.Family);
                        cmd.Parameters.AddWithValue("@Genus", specimen.Genus);
                        cmd.Parameters.AddWithValue("@Species", specimen.Species);
                        cmd.Parameters.AddWithValue("@SubSpecies", specimen.SubSpecies);
                        cmd.Parameters.AddWithValue("@Author", specimen.Author);
                        cmd.Parameters.AddWithValue("@SubAuthor", specimen.SubAuthor);
                        cmd.Parameters.AddWithValue("@Community", specimen.Community);
                        cmd.Parameters.AddWithValue("@SoilType", specimen.SoilType);
                        cmd.Parameters.AddWithValue("@SoilColor", specimen.SoilColor);
                        cmd.Parameters.AddWithValue("@SoilMoisture", specimen.SoilMoisture);
                        cmd.Parameters.AddWithValue("@SIKey", specimen.SIKey.ToString());
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        // deal with image
                        // upload image if they gave a new one
                        if (specimen.MarkedForDeletion)
                        {
                            if (specimen.ImageName != " " && System.IO.File.Exists(specimen.ImageName))
                            {
                                System.IO.File.Delete(specimen.ImageName);
                            }

                            // now that the file has been deleted set the database to match
                            conn = new OleDbConnection();
                            sqlStatement = "UPDATE  [Image] "
                                            + "SET ImageName = ' ' "
                                            + "WHERE [IIKey] = @IIkey;";
                            try
                            {
                                conn.ConnectionString = connStr();
                                cmd = new OleDbCommand(sqlStatement, conn);
                                //parameters are taken in order - not by name
                                cmd.Parameters.AddWithValue("@IIKey", specimen.IIKey);
                                
                                conn.Open();
                                cmd.ExecuteNonQuery();
                            }
                            catch
                            {
                                updateSuccessful = false;
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                        else // specimen.MarkedForDeletion == false
                        {
                            string newImageName = " ";



                            if (specimen.Image != null && specimen.Image.ContentLength > 0) // if an image was selected for uploading
                            {
                                // if imageNameWithoutPath == " " then this specimen did not have an image before this time
                                if (imageNameWithoutPath != " ")
                                {
                                    // delete old file only if this specimen had an old one
                                    System.IO.File.Delete(specimen.ImageName);
                                }
                               

                                // get the file extension from the new file
                                var fileName = Path.GetFileName(specimen.Image.FileName).ToLower();
                                string fileExtension = Path.GetExtension(fileName);

                                // generate a GUID to save the image on the server with a unique name
                                System.Guid imageGUID = System.Guid.NewGuid();

                                // stick them together, will use this later to save in the database
                                newImageName = imageGUID.ToString() + fileExtension;

                                // upload the file
                                entry.Image.SaveAs(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/"), newImageName));

                                // query database to update
                                try
                                {
                                    conn = new OleDbConnection();
                                    sqlStatement = "UPDATE  [Image] "
                                                    + "SET ImageName = @ImageName "
                                                    + "WHERE [IIKey] = @IIkey;";
                                    try
                                    {
                                        conn.ConnectionString = connStr();
                                        cmd = new OleDbCommand(sqlStatement, conn);
                                        //parameters are taken in order - not by name
                                        cmd.Parameters.AddWithValue("@ImageName", newImageName);
                                        cmd.Parameters.AddWithValue("@IIKey", specimen.IIKey);

                                        conn.Open();
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch
                                    {

                                    }
                                    finally
                                    {
                                        conn.Close();
                                    }
                                }
                                catch
                                {
                                    updateSuccessful = false;
                                }
                                finally
                                {
                                    conn.Close();
                                }

                            }
                        }

                        // exceptions would have been thrown if it didn't get this far
                        // only gets set to true if everything worked properly
                        // sadly at this point in the code, there was no rollback, so some
                        // empty fields will be present in a specimen if it got half way through
                        // they will need to be deleted manually through the system
                        updateSuccessful = true;

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);

                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return updateSuccessful;
        }

        public bool deleteASpecimen(string LocationGUID, string ImageNameWithFullPath)
        {
            bool deleteSuccessful = false;
            // the relations in the database should take care of cascading down to Specimen and Acquisition
            // TODO: have not implemented images yet so will need to add that
            OleDbConnection conn = new OleDbConnection();
            String sqlStatement = "DELETE FROM [Location] "
                                    + "WHERE [LocationGUID] = @LocationGUID;";
            try
            {
                conn.ConnectionString = connStr();
                OleDbCommand cmd = new OleDbCommand(sqlStatement, conn);
                //parameters are taken in order - not by name
                cmd.Parameters.AddWithValue("@LocationGUID", LocationGUID);
                conn.Open();
                cmd.ExecuteNonQuery();
                
                // that should have cascaded down to delete the image associated with this specimen
                // but the file still exists, need to delete it
                if (ImageNameWithFullPath != " " && System.IO.File.Exists(ImageNameWithFullPath))
                {
                    System.IO.File.Delete(ImageNameWithFullPath);
                }

                // if no exceptions were thrown deleteSuccessful will get set to true
                deleteSuccessful = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return deleteSuccessful;
        }


        public List<SearchResultViewModel> getSingleSpecimen(string specimenGUID, string locationGUID, int aikey)
        {
            // get all results
            List<SearchResultViewModel> allSpecimens = getAllSpecimensWithAcquisitionAndLocationData(true);


            List<SearchResultViewModel> results = new List<SearchResultViewModel>();
            // find the one we need
            foreach (var result in allSpecimens)
            {
                if(result.SpecimenGUID.Equals(specimenGUID) && result.LocationGUID.Equals(locationGUID) && result.AIKey == aikey)
                {
                    results.Add(result);
                }
            }

            return results;
        }


        public List<User> getAllUsers()
        {
            List<User> users = new List<User>();
            DataSet ds = new DataSet();
            OleDbConnection conn = new OleDbConnection();
            String sqlStatement = "SELECT * "
                                    + "FROM [User] "
                                    + "ORDER BY [UserName];";
            try
            {
                conn.ConnectionString = connStr();
                OleDbCommand cmd = new OleDbCommand(sqlStatement, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                users = convertUsers(ds); // OleDb returns a dataset with a datatable - convert to a class
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return users;
        }

        // *************************************************************
        public User getUser(String userName)
        {
            User user = new User();
            List<User> users = new List<User>();
            DataSet ds = new DataSet();
            OleDbConnection conn = new OleDbConnection();
            String sqlStatement = "SELECT * "
                                    + "FROM [User] "
                                    + "WHERE [UserName] = @userName;";
            try
            {
                conn.ConnectionString = connStr();
                OleDbCommand cmd = new OleDbCommand(sqlStatement, conn);
                cmd.Parameters.AddWithValue("@userName", userName);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                users = convertUsers(ds);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            if (users.Count > 0)
            {
                user =  users[0];
            }

            return user;
        }

        // **************************************************************
        public void saveUser(User user)
        {
            OleDbConnection conn = new OleDbConnection();
            String sqlStatement = "UPDATE  [User] "
                                    + "SET FName = @fName, "
                                    + "LName = @lName "
                                    + "WHERE [UserName] = @userName;";
            try
            {
                conn.ConnectionString = connStr();
                OleDbCommand cmd = new OleDbCommand(sqlStatement, conn);
                //parameters are taken in order - not by name
                cmd.Parameters.AddWithValue("@fName", user.fName);
                cmd.Parameters.AddWithValue("@lName", user.lName);
                cmd.Parameters.AddWithValue("@userName", user.userName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        public bool resetPassword(User user)
        {
            bool result = false;
            OleDbConnection conn = new OleDbConnection();
            String sqlStatement = "UPDATE  [User] "
                                    + "SET UserPassword = 'herbarium' "
                                    + "WHERE [UserName] = @userName;";
            try
            {
                conn.ConnectionString = connStr();
                OleDbCommand cmd = new OleDbCommand(sqlStatement, conn);
                //parameters are taken in order - not by name
                cmd.Parameters.AddWithValue("@userName", user.userName);
                conn.Open();
                cmd.ExecuteNonQuery();
                // if it worked this will be set to true
                result = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }



        public bool setPassword(SetPassword setPassword, string username)
        {
            bool updateWorked = false;
            string hashedPassword = CreateMD5Hash(setPassword.password1);

            OleDbConnection conn = new OleDbConnection();
            String sqlStatement = "UPDATE  [User] "
                                    + "SET UserPassword = @hashedPassword "
                                    + "WHERE [UserName] = @userName;";
            try
            {
                conn.ConnectionString = connStr();
                OleDbCommand cmd = new OleDbCommand(sqlStatement, conn);
                //parameters are taken in order - not by name
                cmd.Parameters.AddWithValue("@hashedPassword", hashedPassword);
                cmd.Parameters.AddWithValue("@userName", username);
                conn.Open();
                cmd.ExecuteNonQuery();
                updateWorked = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return updateWorked;
        }


        public void newSpecimen(NewDatabaseSpecimenEntry entryTemp, string username)
        {

            NewDatabaseSpecimenEntry entry = getRidOfNullStrings(entryTemp);

            // create a guid object so we can generate GUIDs
            System.Guid locationGUID = System.Guid.NewGuid();
            
            System.Guid specimenGUID = System.Guid.NewGuid();

            System.Guid imageGUID = System.Guid.NewGuid();

            string newImageName = " ";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connStr()))
                {
                    connection.Open();

                    // Create the Command.
                    OleDbCommand cmd = new OleDbCommand();

                    // Set the Connection, CommandText and Parameters.
                    cmd.Connection = connection;
                    cmd.CommandText =
                        "INSERT INTO  [Location] (LocationGUID, Lat, Lng, Elevation, Country, County, State, LocationDesc, Township, Sect, Range) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);";
                    cmd.Parameters.Add("LocationGUID", OleDbType.Guid);
                    cmd.Parameters.Add("Lat", OleDbType.Double);
                    cmd.Parameters.Add("Lng", OleDbType.Double);
                    cmd.Parameters.Add("Elevation", OleDbType.Integer);
                    cmd.Parameters.Add("Country", OleDbType.VarWChar);
                    cmd.Parameters.Add("County", OleDbType.VarWChar);
                    cmd.Parameters.Add("State", OleDbType.VarWChar);
                    cmd.Parameters.Add("LocationDesc", OleDbType.VarWChar);
                    cmd.Parameters.Add("Township", OleDbType.VarWChar);
                    cmd.Parameters.Add("Sect", OleDbType.VarWChar);
                    cmd.Parameters.Add("Range", OleDbType.VarWChar);

                    cmd.Parameters[0].Value = locationGUID;
                    cmd.Parameters[1].Value = entry.Lat;
                    cmd.Parameters[2].Value = entry.Lng;
                    cmd.Parameters[3].Value = entry.Elevation;
                    cmd.Parameters[4].Value = entry.Country;
                    cmd.Parameters[5].Value = entry.County;
                    cmd.Parameters[6].Value = entry.State;
                    cmd.Parameters[7].Value = entry.LocationDesc;
                    cmd.Parameters[8].Value = entry.Township;
                    cmd.Parameters[9].Value = entry.Sect;
                    cmd.Parameters[10].Value = entry.Range;

                    // Call  Prepare and ExecuteNonQuery, this is for stored procedures I think? crashes when uncommented
                    //cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    connection.Close();
                }

                using (OleDbConnection connection = new OleDbConnection(connStr()))
                {
                    connection.Open();

                    // Create the Command.
                    OleDbCommand cmd = new OleDbCommand();

                    // Set the Connection, CommandText and Parameters.
                    cmd.Connection = connection;
                    cmd.CommandText =
                        "INSERT INTO[Specimen](SpecimenGUID, LocationGUID, Family, Genus, Species, SubSpecies, Author, SubAuthor, Community, SoilType, SoilColor, SoilMoisture) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);";
                    cmd.Parameters.Add("SpecimenGUID", OleDbType.Guid);
                    cmd.Parameters.Add("LocationGUID", OleDbType.Guid);
                    cmd.Parameters.Add("Family", OleDbType.VarWChar);
                    cmd.Parameters.Add("Genus", OleDbType.VarWChar);
                    cmd.Parameters.Add("Species", OleDbType.VarWChar);
                    cmd.Parameters.Add("SubSpecies", OleDbType.VarWChar);
                    cmd.Parameters.Add("Author", OleDbType.VarWChar);
                    cmd.Parameters.Add("SubAuthor", OleDbType.VarWChar);
                    cmd.Parameters.Add("Community", OleDbType.VarWChar);
                    cmd.Parameters.Add("SoilType", OleDbType.VarWChar);
                    cmd.Parameters.Add("SoilColor", OleDbType.VarWChar);
                    cmd.Parameters.Add("SoilMoisture", OleDbType.VarWChar);

                    cmd.Parameters[0].Value = specimenGUID;
                    cmd.Parameters[1].Value = locationGUID;
                    cmd.Parameters[2].Value = entry.Family;
                    cmd.Parameters[3].Value = entry.Genus;
                    cmd.Parameters[4].Value = entry.Species;
                    cmd.Parameters[5].Value = entry.SubSpecies;
                    cmd.Parameters[6].Value = entry.Author;
                    cmd.Parameters[7].Value = entry.SubAuthor;
                    cmd.Parameters[8].Value = entry.Community;
                    cmd.Parameters[9].Value = entry.SoilType;
                    cmd.Parameters[10].Value = entry.SoilColor;
                    cmd.Parameters[11].Value = entry.SoilMoisture;

                    // Call  Prepare and ExecuteNonQuery, this is for stored procedures I think? crashes when uncommented
                    //cmd.Prepare();

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }

                using (OleDbConnection connection = new OleDbConnection(connStr()))
                {
                    connection.Open();

                    // Create the Command.
                    OleDbCommand cmd = new OleDbCommand();

                    // Set the Connection, CommandText and Parameters.
                    cmd.Connection = connection;
                    cmd.CommandText =
                        "INSERT INTO[Acquisition](AcquisitionNo, SpecimenGUID, EthnobotanicalUse, CollectedDate, CollectorNo, Notes, CollectorName, Deleted, UserName) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?);";
                    cmd.Parameters.Add("AcquisitionNo", OleDbType.Integer);
                    cmd.Parameters.Add("SpecimenGUID", OleDbType.Guid);
                    cmd.Parameters.Add("EthnobotanicalUse", OleDbType.VarWChar);
                    cmd.Parameters.Add("CollectedDate", OleDbType.Date);
                    cmd.Parameters.Add("CollectorNo", OleDbType.VarWChar);
                    cmd.Parameters.Add("Notes", OleDbType.VarWChar);
                    cmd.Parameters.Add("CollectorName", OleDbType.VarWChar);
                    cmd.Parameters.Add("Deleted", OleDbType.Boolean);
                    cmd.Parameters.Add("UserName", OleDbType.VarWChar);

                    cmd.Parameters[0].Value = entry.AcquisitionNo;
                    cmd.Parameters[1].Value = specimenGUID;
                    cmd.Parameters[2].Value = entry.EthnobotanicalUse;
                    cmd.Parameters[3].Value = entry.CollectedDate;
                    cmd.Parameters[4].Value = entry.CollectorNo;
                    cmd.Parameters[5].Value = entry.Notes;
                    cmd.Parameters[6].Value = entry.CollectorName;
                    cmd.Parameters[7].Value = false;
                    cmd.Parameters[8].Value = entry.UserName;
                    // Call  Prepare and ExecuteNonQuery, this is for stored procedures I think? crashes when uncommented
                    //cmd.Prepare();

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }

                // if it worked up to this point let's upload the image that was passed in

               

                if (entry.Image != null && entry.Image.ContentLength > 0)
                {
                    // get the file extension from the file
                    var fileName = Path.GetFileName(entry.Image.FileName).ToLower();
                    string fileExtension = Path.GetExtension(fileName);

                    // generate a GUID to save the image on the server with a unique name

                    // stick them together, will use this later to save in the database
                    newImageName = imageGUID.ToString() + fileExtension;

                    entry.Image.SaveAs(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/"), newImageName));
                }


                // save this image name to the database
                using (OleDbConnection connection = new OleDbConnection(connStr()))
                {
                    connection.Open();

                    // Create the Command.
                    OleDbCommand cmd = new OleDbCommand();

                    // Set the Connection, CommandText and Parameters.
                    cmd.Connection = connection;
                    cmd.CommandText =
                        "INSERT INTO [Image] (ImageName, LocationGUID) VALUES (?, ?);";
                    cmd.Parameters.Add("ImageName", OleDbType.VarWChar);
                    cmd.Parameters.Add("LocationGUID", OleDbType.Guid);

                    cmd.Parameters[0].Value = newImageName;
                    cmd.Parameters[1].Value = locationGUID;

                    // Call  Prepare and ExecuteNonQuery, this is for stored procedures I think? crashes when uncommented
                    //cmd.Prepare();

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                // undo any inserts
                deleteASpecimen(entry.LocationGUID, newImageName);
            }
            finally
            {
                // I beleive the connections get closed in the "using" blocks, the guide I followed didn't manually close their connections
            }
        }

        // create a new user
        public void newUser(User user)
        {
            int admin = 0;

            if(user.admin == true)
            {
                admin = 1;
            }
            else
            {
                admin = 0;
            }
            
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connStr()))
                {
                    connection.Open();

                    // Create the Command.
                    OleDbCommand cmd = new OleDbCommand();

                    // Set the Connection, CommandText and Parameters.
                    cmd.Connection = connection;
                    cmd.CommandText =
                        "INSERT INTO [User] (UserName, FName, LName, UserPassword, Admin) VALUES (?, ?, ?, ?, ?);";
                    cmd.Parameters.Add("UserName", OleDbType.VarWChar);
                    cmd.Parameters.Add("FName", OleDbType.VarWChar);
                    cmd.Parameters.Add("LName", OleDbType.VarWChar);
                    cmd.Parameters.Add("UserPassword", OleDbType.VarWChar);
                    cmd.Parameters.Add("Admin", OleDbType.Boolean);


                    cmd.Parameters[0].Value = user.userName;
                    cmd.Parameters[1].Value = user.fName;
                    cmd.Parameters[2].Value = user.lName;
                    cmd.Parameters[3].Value = "herbarium";
                    cmd.Parameters[4].Value = admin;

                    // Call  Prepare and ExecuteNonQuery, this is for stored procedures I think? crashes when uncommented
                    //cmd.Prepare();

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }
                
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                //conn.Close();
            }
        }
        // ***** DataSet converts to a class for easier use throughout 
        private List<User> convertUsers(DataSet ds)
        {
            List<User> users = new List<User>();

            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    User user = new User();
                    user.userName = row["UserName"].ToString();
                    user.fName = row["FName"].ToString();
                    user.lName = row["LName"].ToString();
                    user.password = row["UserPassword"].ToString();
                    user.admin = (bool) row["Admin"];
                   users.Add(user);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return users;
        }


        private List<SearchResultViewModel> convertSearchResultViewModels(DataSet ds)
        {
            List<SearchResultViewModel> specimens = new List<SearchResultViewModel>();

            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SearchResultViewModel specimen = new SearchResultViewModel();
                    
                    specimen.SpecimenGUID      =            row["SpecimenGUID"].ToString(); // PK
                    specimen.Family            =            row["Family"].ToString();
                    specimen.Genus             =            row["Genus"].ToString();
                    specimen.Species           =            row["Species"].ToString();
                    specimen.SubSpecies        =            row["SubSpecies"].ToString();
                    specimen.Author            =            row["Author"].ToString();
                    specimen.SubAuthor         =            row["SubAuthor"].ToString();
                    specimen.Community         =            row["Community"].ToString();
                    specimen.SoilType          =            row["SoilType"].ToString();
                    specimen.SoilColor         =            row["SoilColor"].ToString();
                    specimen.SoilMoisture      =            row["SoilMoisture"].ToString();
                    specimen.SIKey             = (int)      row["SIKey"];
                                                            
                    specimen.AcquisitionNo     = (int)      row["AcquisitionNo"];
                    specimen.EthnobotanicalUse =            row["EthnobotanicalUse"].ToString();
                    specimen.CollectedDate     = (DateTime) row["CollectedDate"];
                    specimen.CollectorNo       =            row["CollectorNo"].ToString();
                    specimen.Notes             =            row["Notes"].ToString();
                    specimen.CollectorName     =            row["CollectorName"].ToString();
                    specimen.Deleted           = (bool)     row["Deleted"];
                    specimen.AIKey             = (int)      row["AIKey"]; // PK
                    specimen.UserName          =            row["UserName"].ToString();


                    specimen.LocationGUID      =            row["LocationGUID"].ToString(); // PK
                    specimen.Lat               = (double)   row["Lat"];
                    specimen.Lng               = (double)   row["Lng"];
                    specimen.Elevation         = (int)      row["Elevation"];
                    specimen.Country           =            row["Country"].ToString();
                    specimen.County            =            row["County"].ToString();
                    specimen.State             =            row["State"].ToString();
                    specimen.LocationDesc      =            row["LocationDesc"].ToString();
                    specimen.LIKey             = (int)      row["LIKey"];
                    specimen.Township          =            row["Township"].ToString();
                    specimen.Sect              =            row["Sect"].ToString();
                    specimen.Range             =            row["Range"].ToString();

                    specimen.IIKey             = (int)      row["IIKey"];
                    specimen.ImageName         =            row["ImageName"].ToString();


                    specimens.Add(specimen);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return specimens;
        }


        // ********************  static connection string  |DataDirectory| signifies App_Data folder
        public static String connStr()
        {
            return @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=|DataDirectory|\Herbarium.accdb";
        }

        public string CreateMD5Hash(string RawData)
        {
            byte[] hash = System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.ASCII.GetBytes(RawData));
            string str = "";
            byte[] numArray = hash;
            int index = 0;
            while (index < numArray.Length)
            {
                byte num = numArray[index];
                str = str + num.ToString("x2");
                checked { ++index; }
            }
            return str;
        }



        public NewDatabaseSpecimenEntry getRidOfNullStrings(NewDatabaseSpecimenEntry entry)
        {
            NewDatabaseSpecimenEntry specimen = entry;

            if (String.IsNullOrEmpty(specimen.EthnobotanicalUse))
            {
                specimen.EthnobotanicalUse = " ";
            }
            if (String.IsNullOrEmpty(specimen.Notes))
            {
                specimen.Notes = " ";
            }
            if (String.IsNullOrEmpty(specimen.CollectorName))
            {
                specimen.CollectorName = " ";
            }

            if (String.IsNullOrEmpty(specimen.CollectorNo))
            {
                specimen.CollectorNo = " ";
            }
            if (String.IsNullOrEmpty(specimen.Country))
            {
                specimen.Country = " ";
            }
            if (String.IsNullOrEmpty(specimen.County))
            {
                specimen.County = " ";
            }
            if (String.IsNullOrEmpty(specimen.State))
            {
                specimen.State = " ";
            }
            if (String.IsNullOrEmpty(specimen.LocationDesc))
            {
                specimen.LocationDesc = " ";
            }
            if (String.IsNullOrEmpty(specimen.Township))
            {
                specimen.Township = " ";
            }
            if (String.IsNullOrEmpty(specimen.Family))
            {
                specimen.Family = " ";
            }
            if (String.IsNullOrEmpty(specimen.Genus))
            {
                specimen.Genus = " ";
            }
            if (String.IsNullOrEmpty(specimen.Species))
            {
                specimen.Species = " ";
            }
            if (String.IsNullOrEmpty(specimen.SubSpecies))
            {
                specimen.SubSpecies = " ";
            }
            if (String.IsNullOrEmpty(specimen.Author))
            {
                specimen.Author = " ";
            }
            if (String.IsNullOrEmpty(specimen.SubAuthor))
            {
                specimen.SubAuthor = " ";
            }
            if (String.IsNullOrEmpty(specimen.Community))
            {
                specimen.Community = " ";
            }
            if (String.IsNullOrEmpty(specimen.SoilType))
            {
                specimen.SoilType = " ";
            }
            if (String.IsNullOrEmpty(specimen.SoilColor))
            {
                specimen.SoilColor = " ";
            }
            if (String.IsNullOrEmpty(specimen.SoilMoisture))
            {
                specimen.SoilMoisture = " ";
            }
            if (String.IsNullOrEmpty(specimen.Range))
            {
                specimen.Range = " ";
            }
            if (String.IsNullOrEmpty(specimen.Sect))
            {
                specimen.Sect = " ";
            }
            if (String.IsNullOrEmpty(specimen.UserName))
            {
                specimen.UserName = " ";
            }

            return specimen;
        }
    }


}