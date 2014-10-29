using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

//This is neccesary in order to "navigate" 
//throught the class members
using System.Reflection; 

namespace KKK_Zusterna.Helper
{
    public class DataTableToList<T>
    {

        public List<T> FromDataTableToList(DataTable dataTable)
        {
            try
            {

                //This create a new list with the same type of the generic class
                List<T> genericList = new List<T>();
                //Obtains the type of the generic class
                Type t = typeof(T);

                //Obtains the properties definition of the generic class.
                //With this, we are going to know the property names of the class
                PropertyInfo[] pi = t.GetProperties();

                //For each row in the datatable

                foreach (DataRow row in dataTable.Rows)
                {
                    //Create a new instance of the generic class
                    object defaultInstance = Activator.CreateInstance(t);
                    //For each property in the properties of the class
                    foreach (PropertyInfo prop in pi)
                    {
                        try
                        {
                            //Get the value of the row according to the field name
                            //Remember that the classïs members and the tableïs field names
                            //must be identical
                            object columnvalue = row[prop.Name];
                            //Know check if the value is null. 
                            //If not, it will be added to the instance
                            if (columnvalue != DBNull.Value)
                            {
                                //Set the value dinamically. Now you need to pass as an argument
                                //an instance class of the generic class. This instance has been
                                //created with Activator.CreateInstance(t)
                                prop.SetValue(defaultInstance, columnvalue, null);
                            }                           
                        }
                        catch (Exception ex)
                        {
                            throw (ex);
                        }
                    }
                    //Now, create a class of the same type of the generic class. 
                    //Then a conversion itïs done to set the value
                    T myclass = (T)defaultInstance;
                    //Add the generic instance to the generic list
                    genericList.Add(myclass);
                }
                //At this moment, the generic list contains all de datatable values
                return genericList;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    
    }

}