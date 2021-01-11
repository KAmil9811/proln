import { Link } from 'react-router-dom';
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
                console.log(json.length);
                console.log(json);

                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        who: json[i].login,
                        what: json[i].description,
                        when: json[i].data
                    })
                }




                this.setState({
                    table333: {
                        columns: [
                            {
                                label: 'Kto',
                                field: 'who',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Co',
                                field: 'what',
                                sort: 'asc',
                                width: 250
                            },
                            {
                                label: 'Kiedy',
                                field: 'when',
                                sort: 'asc',
                                width: 200
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


                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={15}
                pagesAmount={10}
                data={this.state.table333}
                searchTop
                materialSearch
                searchBottom={false}
                responsive
                bordered
                sortable
                theadTextWhite
                theadTextWhite
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