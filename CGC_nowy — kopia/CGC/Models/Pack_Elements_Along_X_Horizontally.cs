//using System;
//using System.ComponentModel;
//using System.Windows.Forms;

//namespace Packing_two_dimensional_rectangular_elements_at_orthogonal_table
//{
//    public class Pack_Elements_Along_X_Horizontally
//    {
//        //  
//        // Tables  
//        //  
//        DataGridView ELEMENT_LIST;
//        DataGridView POSITION_LIST;
//        DataGridView TABLE_LIST;
//        //  
//        // Variables  
//        //  
//        int Tl, Tw; // table length and width  //szkło
//        int n, k, p, i; // counters  //liczniki
//        int x, l, w, a, f; // element index, length, width, area and fitting factor   // cos
//        int Lmin, Wmin; // smalest element length and width  // wymiary najmniejszy item
//        int X11, Y11, L11, W11, A11; // new position I coordinates, length, width and area  
//        int X12, Y12, L12, W12, A12; // new position II coordinates, length, width and area  
//        int p_X; // old position X  
//        int p_Y; // old position Y  
//        int Pl; // old position length  
//        int Pw; // old position width  
//        bool P_ok; // new position 'is adequate' state flag  
//        const string format = "0000000000"; // number format  
//        //  
//        //  
//        //  
//        void Initialize_Tables()
//        {
//            //  
//            // Create table ELEMENT_LIST  
//            //  
//            ELEMENT_LIST = new DataGridView();
//            //  
//            // Add columns  
//            //  
//            // DataGridView.Columns.Add("Column Name", "Column Header Text");  
//            //  
//            ELEMENT_LIST.Columns.Add("E_Index", "Element index");
//            ELEMENT_LIST.Columns.Add("E_Length", "Element length");
//            ELEMENT_LIST.Columns.Add("E_Width", "Element width");
//            ELEMENT_LIST.Columns.Add("E_Area", "Element area");
//            ELEMENT_LIST.Columns.Add("E_Fitting", "Element fiting faktor");
//            //  
//            // Add SortCompare event handler  
//            //  
//            ELEMENT_LIST.SortCompare +=
//                new System.Windows.Forms.DataGridViewSortCompareEventHandler(E_SortCompare);
//            //  
//            //  
//            //  
//            //  
//            // Create table POSITION LIST  
//            //  
//            POSITION_LIST = new DataGridView();
//            //  
//            // Add columns  
//            //  
//            // DataGridView.Columns.Add("Column Name", "Column Header Text");  
//            //  
//            POSITION_LIST.Columns.Add("P_X", "Position X");
//            POSITION_LIST.Columns.Add("P_Y", "Position Y");
//            POSITION_LIST.Columns.Add("P_Length", "Position length");
//            POSITION_LIST.Columns.Add("P_Width", "Position width");
//            //  
//            // Add SortCompare event handler  
//            //  
//            POSITION_LIST.SortCompare +=
//                new System.Windows.Forms.DataGridViewSortCompareEventHandler(P_SortCompare);
//            //  
//            //  
//            //  
//            //  
//            // Create table TABLE LIST  
//            //  
//            TABLE_LIST = new DataGridView();
//            //  
//            // Add columns  
//            //  
//            // DataGridView.Columns.Add("Column Name", "Column Header Text");  
//            //  
//            TABLE_LIST.Columns.Add("E_I", "Element index");
//            TABLE_LIST.Columns.Add("E_X", "Element X");
//            TABLE_LIST.Columns.Add("E_Y", "Element Y");
//            TABLE_LIST.Columns.Add("E_Length", "Element length");
//            TABLE_LIST.Columns.Add("E_Width", "Element width");
//            //  
//            //  
//            //  
//        }
//        void Initialize_Variables()
//        {
//            Tl = 0;
//            Tw = 0;
//            n = 0;
//            k = 0;
//            p = 0;
//            i = 0;
//            x = 0;
//            l = 0;
//            w = 0;
//            a = 0;
//            f = 0;
//            Lmin = 0;
//            Wmin = 0;
//            X11 = 0;
//            Y11 = 0;
//            L11 = 0;
//            W11 = 0;
//            A11 = 0;
//            X12 = 0;
//            Y12 = 0;
//            L12 = 0;
//            W12 = 0;
//            A12 = 0;
//            p_X = 0;
//            p_Y = 0;
//            Pl = 0;
//            Pw = 0;
//            P_ok = false;
//        }
//        void E_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
//        {
//            //  
//            // SortCompare event handler  
//            //  
//            //  
//            // Sort table by selected column then by forth then by second  
//            // then by third then by first column in the selected Sort Direction  
//            //  
//            // Compare values inside cells in selected column  
//            e.SortResult = System.String.CompareOrdinal(
//                e.CellValue1.ToString(), e.CellValue2.ToString());
//            // If values are equal  
//            // Compare values inside cells in forth column  
//            if (e.SortResult == 0)
//            {
//                e.SortResult = System.String.CompareOrdinal(
//                    ELEMENT_LIST.Rows[e.RowIndex1].Cells[3].Value.ToString(),
//                    ELEMENT_LIST.Rows[e.RowIndex2].Cells[3].Value.ToString());
//                // If values are equal  
//                // Compare values inside cells in second column  
//                if (e.SortResult == 0)
//                {
//                    e.SortResult = System.String.CompareOrdinal(
//                        ELEMENT_LIST.Rows[e.RowIndex1].Cells[1].Value.ToString(),
//                        ELEMENT_LIST.Rows[e.RowIndex2].Cells[1].Value.ToString());
//                    // If values are equal  
//                    // Compare values inside cells in third column  
//                    if (e.SortResult == 0)
//                    {
//                        e.SortResult = System.String.CompareOrdinal(
//                            ELEMENT_LIST.Rows[e.RowIndex1].Cells[2].Value.ToString(),
//                            ELEMENT_LIST.Rows[e.RowIndex2].Cells[2].Value.ToString());
//                        // If values are equal  
//                        // Compare values inside cells in first column  
//                        // in oposite sort direction than selected  
//                        if (e.SortResult == 0)
//                        {
//                            e.SortResult = System.String.CompareOrdinal(
//                                ELEMENT_LIST.Rows[e.RowIndex2].Cells[0].Value.ToString(),
//                                ELEMENT_LIST.Rows[e.RowIndex1].Cells[0].Value.ToString());
//                        }
//                    }
//                }
//            }
//            e.Handled = true;
//        }
//        void P_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
//        {
//            //  
//            // SortCompare event handler  
//            //  
//            // Sort table by selected column then by first column  
//            // in the selected Sort Direction  
//            //  
//            // Compare values inside cells in selected column  
//            e.SortResult = System.String.CompareOrdinal(
//                e.CellValue1.ToString(), e.CellValue2.ToString());
//            // If values are equal  
//            // Compare values inside cells in first column  
//            if (e.SortResult == 0)
//            {
//                e.SortResult = System.String.CompareOrdinal(
//                    POSITION_LIST.Rows[e.RowIndex1].Cells[0].Value.ToString(),
//                    POSITION_LIST.Rows[e.RowIndex2].Cells[0].Value.ToString());
//            }
//            e.Handled = true;
//        }
//        public DataGridView Pack_Elements(int Table_length, int Table_width, DataGridView Elements)
//        {
//            //  
//            // Packing elements in sequence along X axis of table,  
//            // with horizontal orientation exclusively.  
//            //  
//            // Algorithm has optimisation for adjusting area  
//            // of positions along X axes  
//            //  
//            //  
//            // This method needs three arguments and returns one argument.  
//            // Table_length, Table_width and DataGridView Elements.  
//            //  
//            // Structure of DataGridView that is going to be passed as argument,  
//            // from caller method, must be the same as structure of  
//            // DataGridView ELEMENT_LIST.  
//            //  
//            // Method returns DataGridView TABLE_LIST with data  
//            // of packed elements: index, x, y, length and width.  
//            //  
//            //  
//            //  
//            //  
//            Initialize_Tables();
//            //  
//            //  
//            //  
//            Initialize_Variables();
//            //  
//            // Set table length and width  
//            //  
//            Tl = Table_length;
//            Tw = Table_width;
//            //  
//            // Copy element dimensions from DataGridView Elements  
//            // to ELEMENT_LIST table and format it's values  
//            //  
//            // Set elements counter to zero  
//            n = 0;
//            // Set table row counter to zero  
//            i = 0;
//            // Fill ELEMENT_LIST  
//            for (i = 0; i < Elements.RowCount; i++)
//            {
//                if (Elements[1, i].Value != null)
//                {
//                    ELEMENT_LIST.Rows.Add();
//                    ELEMENT_LIST[0, i].Value = (int.Parse(Elements[0, i].Value.ToString())).ToString(format);
//                    ELEMENT_LIST[1, i].Value = (int.Parse(Elements[1, i].Value.ToString())).ToString(format);
//                    ELEMENT_LIST[2, i].Value = (int.Parse(Elements[2, i].Value.ToString())).ToString(format);
//                    ELEMENT_LIST[3, i].Value = (int.Parse(Elements[3, i].Value.ToString())).ToString(format);
//                    ELEMENT_LIST[4, i].Value = (int.Parse(Elements[4, i].Value.ToString())).ToString(format);
//                    // Increase elements counter by one  
//                    n++;
//                }
//            }
//            //  
//            // Sort elements in descending sort order  
//            // first by area then by length then by width of elements  
//            //  
//            ELEMENT_LIST.Sort(ELEMENT_LIST.Columns[3], ListSortDirection.Descending);
//            // Set position coordinates to origin  
//            p_X = 0;
//            p_Y = 0;
//            //  
//            // Enter coordinates and dimensions  
//            // of first position in table 'POSITION_LIST'  
//            //  
//            POSITION_LIST.Rows.Add(p_X.ToString(format), // X coordinate  
//                p_Y.ToString(format), // Y coordinate  
//                Tl.ToString(format), // table lenght  
//                Tw.ToString(format)); // table width  
//            // Set position counter to one  
//            k = 1;
//            // Set counter of packed elements to zero  
//            p = 0;
//            //  
//            // Pack elements while there is free positions and  
//            // elements for packing  
//            //  
//            while (k > 0 && n > 0)
//            {
//                // Sort positions in ascending sort order first by Y, and then by X axes  
//                POSITION_LIST.Sort(POSITION_LIST.Columns[1], ListSortDirection.Ascending);
//                // Get length and width of first position in 'POSITION_LIST'  
//                Pl = int.Parse(POSITION_LIST[2, 0].Value.ToString());
//                Pw = int.Parse(POSITION_LIST[3, 0].Value.ToString());
//                // Calculate fitting factor for each element for the first position  
//                for (i = 0; i < n; i++)
//                {
//                    // Element length  
//                    l = int.Parse(ELEMENT_LIST[1, i].Value.ToString());
//                    // Element width  
//                    w = int.Parse(ELEMENT_LIST[2, i].Value.ToString());
//                    // Set fitting factor initial value  
//                    f = 0;
//                    // Does element fits inside position  
//                    if (Pl < l || Pw < w)
//                    {
//                        // If not  
//                        f = 0;
//                    }
//                    else
//                    {
//                        // If yes  
//                        f++;
//                        //  
//                        if (Pl == l)
//                        {
//                            // If element fits exactly across lenght of position  
//                            f++;
//                        }
//                        //  
//                        if (Pw == w)
//                        {
//                            // If element fits exactly across width of position  
//                            f++;
//                        }
//                    }
//                    // Enter fitting factor value inside ELEMENT_LIST table  
//                    ELEMENT_LIST[4, i].Value = f.ToString(format);
//                }
//                //  
//                // Sort elements in descending sort order first by fitting factor value,  
//                // then by area then by length then by width of elements  
//                //  
//                ELEMENT_LIST.Sort(ELEMENT_LIST.Columns[4], ListSortDirection.Descending);
//                //  
//                // Get fitting factor value for the first element  
//                //  
//                f = int.Parse(ELEMENT_LIST[4, 0].Value.ToString());
//                //  
//                // If fitting factor is greater than zero  
//                // element can be packed inside selected position  
//                //  
//                if (f > 0)
//                {
//                    //  
//                    // Get coordinates of selected position  
//                    //  
//                    p_X = int.Parse(POSITION_LIST[0, 0].Value.ToString());
//                    p_Y = int.Parse(POSITION_LIST[1, 0].Value.ToString());
//                    // Get index, length and width of packed element  
//                    x = int.Parse(ELEMENT_LIST[0, 0].Value.ToString());
//                    l = int.Parse(ELEMENT_LIST[1, 0].Value.ToString());
//                    w = int.Parse(ELEMENT_LIST[2, 0].Value.ToString());
//                    //  
//                    // Enter coordinates of position,  
//                    // element dimensions and index,  
//                    // at TABLE LIST  
//                    //  
//                    TABLE_LIST.Rows.Add();
//                    TABLE_LIST[0, p].Value = x.ToString(); // index  
//                    TABLE_LIST[1, p].Value = p_X.ToString(); // position x  
//                    TABLE_LIST[2, p].Value = p_Y.ToString(); // position y  
//                    TABLE_LIST[3, p].Value = l.ToString(); // element length  
//                    TABLE_LIST[4, p].Value = w.ToString(); // element width  
//                    // Increase packed elements counter by one  
//                    p++;
//                    // Erase packed element from list of elements  
//                    ELEMENT_LIST.Rows.RemoveAt(0);
//                    // Decrease elements counter by one  
//                    n--;
//                    //  
//                    // If there is free elements left  
//                    // seek for new positions  
//                    //  
//                    if (n > 0)
//                    {
//                        //  
//                        // Sort elements in descending sort order,  
//                        // first by area then by length then by width of elements  
//                        //  
//                        // This step is necessary for faster searching  
//                        // for element that can fit inside new positions  
//                        //  
//                        // And the elements are sorted accordingly to demand of  
//                        // second restriction in algorithm development  
//                        // for the next step in packing process  
//                        //  
//                        ELEMENT_LIST.Sort(ELEMENT_LIST.Columns[3], ListSortDirection.Descending);
//                        // Set new position 'is adequate' state flag  
//                        P_ok = true;
//                        //  
//                        // New position II with coordinates  
//                        // at upper left corner of previous  
//                        // packed element  
//                        //  
//                        if (Pw > w)
//                        {
//                            //  
//                            // Initialize local variables  
//                            //  
//                            X12 = 0;
//                            Y12 = 0;
//                            L12 = 0;
//                            W12 = 0;
//                            A12 = 0;
//                            // Set new position 'is adequate' state flag  
//                            P_ok = false;
//                            //  
//                            // Coordinates of new position II  
//                            //  
//                            X12 = p_X;
//                            Y12 = p_Y + w;
//                            //  
//                            // Length, width and area of new position II  
//                            //  
//                            L12 = Tl - X12;
//                            W12 = Pw - w;
//                            A12 = L12 * W12;
//                            //  
//                            // Find is there element left that can fit inside position  
//                            //  
//                            // Set ELEMENT_LIST table counter value  
//                            i = n - 1;
//                            //  
//                            // Get area of the last element in table  
//                            //  
//                            a = int.Parse(ELEMENT_LIST[3, i].Value.ToString());
//                            while (a <= A12 && i > -1)
//                            {
//                                //  
//                                // Get next element area  
//                                //  
//                                a = int.Parse(ELEMENT_LIST[3, i].Value.ToString());
//                                //  
//                                // Get element length and width value  
//                                //  
//                                Lmin = int.Parse(ELEMENT_LIST[1, i].Value.ToString());
//                                Wmin = int.Parse(ELEMENT_LIST[2, i].Value.ToString());
//                                //  
//                                // If element can fit  
//                                // at observed position,  
//                                // set new position II at list of positions  
//                                //  
//                                if (L12 >= Lmin && W12 >= Wmin)
//                                {
//                                    POSITION_LIST.Rows.Add(X12.ToString(format),
//                                        Y12.ToString(format),
//                                        L12.ToString(format),
//                                        W12.ToString(format));
//                                    // Increase positions counter by one  
//                                    k++;
//                                    // Set new position 'is adequate' state flag  
//                                    P_ok = true;
//                                    // Set counter value to loop exit value  
//                                    i = -1;
//                                }
//                                // Decrease elements counter by one  
//                                i--;
//                            }
//                        }
//                        //  
//                        // New position I with coordinates  
//                        // at lower right corner of previous  
//                        // packed element  
//                        //  
//                        if (Pl > l)
//                        {
//                            //  
//                            // Initialize local variables  
//                            //  
//                            X11 = 0;
//                            Y11 = 0;
//                            L11 = 0;
//                            W11 = 0;
//                            A11 = 0;
//                            //  
//                            // Coordinates of new position I  
//                            //  
//                            X11 = p_X + l;
//                            Y11 = p_Y;
//                            //  
//                            // Length, width and area of new position I  
//                            //  
//                            L11 = Tl - X11;
//                            W11 = w;
//                            A11 = L11 * W11;
//                            //  
//                            // If position two is inadeqate,  
//                            // adjust width of position one  
//                            //  
//                            if (!P_ok)
//                            {
//                                W11 = W11 + W12;
//                                A11 = L11 * W11;
//                            }
//                            //  
//                            // Find is there element left that can fit inside position  
//                            //  
//                            // Set element counter value  
//                            i = n - 1;
//                            //  
//                            // Get area of the last element in table  
//                            //  
//                            a = int.Parse(ELEMENT_LIST[3, i].Value.ToString());
//                            while (a <= A11 && i > -1)
//                            {
//                                //  
//                                // Get next element area  
//                                //  
//                                a = int.Parse(ELEMENT_LIST[3, i].Value.ToString());
//                                //  
//                                // Get element length and width value  
//                                //  
//                                Lmin = int.Parse(ELEMENT_LIST[1, i].Value.ToString());
//                                Wmin = int.Parse(ELEMENT_LIST[2, i].Value.ToString());
//                                //  
//                                // If there is element that can fit  
//                                // at position,  
//                                // set new position I at list of positions  
//                                //  
//                                if (L11 >= Lmin && W11 >= Wmin)
//                                {
//                                    POSITION_LIST.Rows.Add(X11.ToString(format),
//                                        Y11.ToString(format),
//                                        L11.ToString(format),
//                                        W11.ToString(format));
//                                    // Increase positions counter by one  
//                                    k++;
//                                    // Set counter value to loop exit value  
//                                    i = -1;
//                                }
//                                // Decrease elements counter by one  
//                                i--;
//                            }
//                        }
//                    }
//                }
//                //  
//                // Is there free elements left for packing  
//                //  
//                if (n > 0)
//                {
//                    //  
//                    // Erase first position from list  
//                    //  
//                    POSITION_LIST.Rows.RemoveAt(0);
//                    // Decrease positions counter by one  
//                    k--;
//                }
//            }
//            //  
//            // End of packing process  
//            //  
//            return TABLE_LIST;
//        }
//    }
//}