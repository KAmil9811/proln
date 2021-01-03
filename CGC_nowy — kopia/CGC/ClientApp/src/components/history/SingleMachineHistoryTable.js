import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { Link } from 'react-router-dom';
import { Route } from 'react-router-dom';



export class SingleMachineHistoryTable extends Component {
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
        const receiver = {
            machines: {
                no: sessionStorage.getItem('no')
            }
        }
        console.log(receiver)

        fetch(`api/Machine/Return_Machines_History`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
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
                        when: json[i].date
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
            <MDBDataTable

                bordered
                small
                data={this.state.table333}
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