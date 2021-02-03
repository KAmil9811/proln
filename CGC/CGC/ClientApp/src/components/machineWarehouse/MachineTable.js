import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import './MachineTable.css'
import { Link } from 'react-router-dom';
import Sidebar from '../Sidebar';


export class MachineTable extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },
        };
    }



    componentDidMount() {
        var table2 = [];
        fetch(`api/Machine/Return_All_Machines`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
               
                for (var i = 0; i < json.length; i++) {
                    var deleted = '';
                    if (json[i].stan === false) {
                        deleted = 'Aktywna'
                    }
                    else {
                        deleted = 'Usunięta'
                    }
                    if (sessionStorage.getItem('machineMenagment') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
                        table2.push({
                            number: json[i].no,
                            status: json[i].status,
                            type: json[i].type,
                            deleted: deleted,
                            chstatus: <button type="button" className="info_t" id={i} onClick={
                                (e) => {
                                    this.machineBroken(table2[e.target.id].number, table2[e.target.id].status)
                                }}
                            >Change status</button>,
                            delete: <button className="danger_t" id={i} onClick={(e) => { this.delete(table2[e.target.id].number, table2[e.target.id].deleted) }}> Delete/Restore  </button>,

                            action: <Link to="/single_machine_history"><button className="prim_t" id={json[i].no} onClick={(e) => { this.single(e.target.id) }}>History</button></Link>
                        })
                    }
                    else {
                    table2.push({
                        number: json[i].no,
                        status: json[i].status,
                        type: json[i].type,
                        deleted: deleted,
                    })
                    }
                };
                if (sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
                    this.setState({
                        table: {
                            columns: [
                                {
                                    label: 'No.',
                                    field: 'number',
                                    sort: 'asc',
                                    width: 30
                                },
                                {
                                    label: 'Type',
                                    field: 'type',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Status',
                                    field: 'status',
                                    sort: 'asc',
                                    width: 150
                                },
                                {
                                    label: 'Change status',
                                    field: 'chstatus',
                                    width: 30
                                },
                                {
                                    label: 'Stan',
                                    field: 'deleted',
                                    width: 30
                                },
                                {
                                    label: 'Delete',
                                    field: 'delete',
                                    width: 30
                                },
                                {
                                    label: 'History',
                                    field: 'action',
                                    width: 100
                                },
                            ],
                            rows: table2
                        }
                    });
                }
                else {
this.setState({
                    table: {
                        columns: [
                            {
                                label: 'No.',
                                field: 'number',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Type',
                                field: 'type',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Status',
                                field: 'status',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Stan',
                                field: 'deleted',
                                width: 30
                            },
                        ],
                        rows: table2
                    }
                });
                }
                
            })
    };
    single(number) {
        sessionStorage.setItem('no', number)
    }
    machineBroken(id, status ) {
        const receiver = {
            user: {
                login: sessionStorage.getItem('login')
            },
            machines: {
                no: id,
                status: status
            }
        }
        

        if (status === 'Ready') { //Zmieniamy na broken
            fetch(`api/Machine/Change_Status_Machine`, {
                method: "post",
                body: JSON.stringify(receiver),
                headers: {
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then(json => {
                
                    return (json);
                })
                .then(json => {
                    alert("Machine is broken!")
                })
                .then(json => {
                    window.location.reload();
                })
        }
        else if (status === 'InUse') { ///w użyciu 
            alert("Machine is in use, you can't change status!")
        }
        else {
            fetch(`api/Machine/Change_Status_Machine`, {
                method: "post",
                body: JSON.stringify(receiver),
                headers: {
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then(json => {
         
                    return (json);
                })
                .then(json => {
                    alert("Machine is fixed")
                })
                .then(json => {
                    window.location.reload();
                })
        }
    }

    delete(id, deleted) {
        const receiver = {
            user: {
                login: sessionStorage.getItem('login')
            },
            machines: {
                no: id
            }
        }
       

        if (deleted === 'Aktywna') {
            fetch(`api/Machine/Remove_Machine`, {
                method: "post",
                body: JSON.stringify(receiver),
                headers: {
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then(json => {
                 
                    return (json);
                })
                .then(json => {
                    alert("You deleted machine")
                })
                .then(json => {
                    window.location.reload();
                })
        }
        else {
            fetch(`api/Machine/Restore_Machine`, {
                method: "post",
                body: JSON.stringify(receiver),
                headers: {
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then(json => {
                
                    return (json);
                })
                .then(json => {
                    alert("You activated machine")
                })
                .then(json => {
                    window.location.reload();
                })
        }


    }

    table() {
        return (
            <MDBDataTableV5


                data={this.state.table}
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