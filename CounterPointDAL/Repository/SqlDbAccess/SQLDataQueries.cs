using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using CounterPointDAL.MusicStoreDomainModel;

namespace CounterPointDAL.Repository.SqlDbAccess
{
    public class SQLDataQueries
    {
        public IEnumerable<MusicCD> GetAllMusicCDs()
        {
            var musicData = new DataSet();
            var numberOfMusicCDs = 0;

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CounterPointMusicPortal"].ConnectionString))
            {
                using (var command = new SqlCommand("sp_getAllMusicCDs", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (var adapter = new SqlDataAdapter())
                        {
                            adapter.SelectCommand = command;

                            adapter.Fill(musicData);
                        }

                    }
                    catch (Exception e)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }

            if (musicData.Tables.Count > 0 && musicData.Tables[0].Rows.Count > numberOfMusicCDs)
            {
                return GetMusicCDsFromDataSet(musicData.Tables[0]);
            }
            else return null;
        }

        public MusicCD GetMusicCDByProductID(int productId)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CounterPointMusicPortal"].ConnectionString))
            {
                using (var command = new SqlCommand("sp_getMusicByProductId", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = productId;
                        connection.Open();

                        var reader = command.ExecuteReader();
                        var musicCD = new MusicCD();

                        while (reader.Read())
                        {
                            musicCD.AlbumTitle = (string)reader["title"];
                            musicCD.Artist = (string)reader["artist"];
                            musicCD.CatalogueNumber = (string)reader["catalogue_number"];
                            musicCD.ProductId = (int)reader["product_id"];
                        }
                        return musicCD;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }
        }

        public SalesLine GetSalesLineByProductID(int productId)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CounterPointMusicPortal"].ConnectionString))
            {
                using (var command = new SqlCommand("sp_getSalesLineByProductId", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        var par1 = new SqlParameter("@product_id", SqlDbType.Int);
                        par1.Direction = ParameterDirection.InputOutput;
                        par1.Value = productId;

                        var par2 = new SqlParameter("@date_sold", SqlDbType.DateTime);
                        par2.Direction = ParameterDirection.Output;
                        par2.Value = DateTime.Now;

                        var par3 = new SqlParameter("@units_sold", SqlDbType.Int);
                        par3.Direction = ParameterDirection.Output;
                        par3.Value = -1;

                        var par4 = new SqlParameter("@sales_line_id", SqlDbType.Int);
                        par4.Direction = ParameterDirection.Output;
                        par4.Value = -1;

                        command.Parameters.Add(par1);
                        command.Parameters.Add(par2);
                        command.Parameters.Add(par3);
                        command.Parameters.Add(par4);
                        connection.Open();

                        var reader = command.ExecuteNonQuery();

                        var saleLine = new SalesLine();
                        saleLine.ProductId = (int)par1.Value;
                        if (par2.Value != DBNull.Value)
                            saleLine.DateSold = (DateTime?)par2.Value;
                        else saleLine.DateSold = null;
                        if (par3.Value != DBNull.Value)
                            saleLine.UnitsSold = (int)par3.Value;
                        else saleLine.UnitsSold = 0;
                        if (par4.Value != DBNull.Value)
                            saleLine.salesLineId = (int)par4.Value;
                        else saleLine.salesLineId = -1;
                        return saleLine;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }
        }

        public int? AddSalesLineToProduct(SalesLine saleLine, MusicCD product)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CounterPointMusicPortal"].ConnectionString))
            {
                using (var command = new SqlCommand("sp_AddSalesLineToProduct", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = product.ProductId;
                        command.Parameters.Add(new SqlParameter("@dateSold", SqlDbType.DateTime)).Value = saleLine.DateSold;
                        command.Parameters.Add(new SqlParameter("@unitsSold", SqlDbType.Int)).Value = saleLine.UnitsSold;
                        connection.Open();

                        var salesId = (int)command.ExecuteScalar();
                        return salesId;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }

        }

        public bool? DeleteSaleLineForProduct(SalesLine saleLine, MusicCD product)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CounterPointMusicPortal"].ConnectionString))
            {
                using (var command = new SqlCommand("sp_DeleteSalesLineForProduct", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = product.ProductId;
                        command.Parameters.Add(new SqlParameter("@dateSold", SqlDbType.DateTime)).Value = saleLine.DateSold;
                        connection.Open();

                        var salesId = (int)command.ExecuteScalar();
                        return salesId > 0;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }
        }
        private IList<MusicCD> GetMusicCDsFromDataSet(DataTable musicData)
        {
            var musicCollection = new List<MusicCD>();

            for (int i = 0; i < musicData.Rows.Count; i++)
            {
                var cd = new MusicCD
                    {
                        AlbumTitle = (string)musicData.Rows[i]["title"],
                        Artist = (string)musicData.Rows[i]["artist"],
                        CatalogueNumber = (string)musicData.Rows[i]["catalogue_number"],
                        ProductId = (int)musicData.Rows[i]["product_id"]
                    };

                musicCollection.Add(cd);
            }

            return musicCollection;
        }
    }
}
