﻿import { Link } from 'react-router-dom';
import React, { Component } from 'react';

import { MDBDataTableV5 } from 'mdbreact';
import Sidebar from '../Sidebar';

export class AllMachineHistoryTable extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table333: {
                columns: [],
                rows: []
            },
        };
    }



    componentDidMount() {
        var table2 = [];


        fetch(`api/Machine/Return_All_Machines_History`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
               

                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        who: json[i].login,
                        no: json[i].no,
                        what: json[i].description,
                        when: json[i].date
                    })
                }




                this.setState({
                    table333: {
                        columns: [
                            {
                                label: 'Who',
                                field: 'who',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'No.',
                                field: 'no',
                                sort: 'asc',
                                width: 50
                            },
                            {
                                label: 'What',
                                field: 'what',
                                sort: 'asc',
                                width: 250
                            },
                            {
                                label: 'When',
                                field: 'when',
                                sort: 'asc',
                                width: 150
                            },
                            
                            /*{
                                label: 'Uprawnienia',
                                field: 'permissions',
                                sort: 'asc',
                                width: 100
                            },
                            {
                                label: 'Akcja',
                                field: 'action',
                                width: 100
                            },
                            {
                                label: 'Status konta',
                                field: 'deleted',
                                width: 100
                            },
                            {
                                label: 'Usuń',
                                field: 'del',
                                width: 100
                            }*/
                        ],
                        rows: table2
                    }
                });

            })

    };



    table() {
        return (
            <MDBDataTableV5
                data={this.state.table333}
                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={10}
                pagesAmount={10}
                searchTop
                materialSearch
                searchBottom={false}
                responsive
                bordered
                paginationLabel={["Previous", "Next"]}
                sortable
                // small
                theadTextWhite
                theadTextWhite
                className="table_corection"
            />



        )
    }

    render() {
        let xd = this.table();
        return (
            <div>
                {xd}
            </div>
        )
    }

}