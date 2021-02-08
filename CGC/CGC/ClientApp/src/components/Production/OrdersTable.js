﻿import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import Sidebar from '../Sidebar';


export class OrdersTable extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },
        };
    }




    //`api/Order/Return_All_Orders`
    componentDidMount() {
        var table2 = [];
        fetch(`api/Cut/Return_Orders_To_Cut`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        number: json[i].id_Order,
                        //status: json[i].status,
                        klient: json[i].owner,
                        x: json[i].stan,
                        priority: json[i].priority,
                        deadline: json[i].deadline,
                        items: json[i].items,
                        choose: <Link to="/ready_packages"> <button className="success_t" id={i}
                            onClick={(e) => {
                                //this.chooseOrder( table2[e.target.id].number);
                                sessionStorage.setItem('orderId2', table2[e.target.id].number)

                            }
                            } > Select </button></Link>



                    })
                };
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
                                label: 'Client',
                                field: 'klient',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'X/Y/Z',
                                field: 'x',
                                sort: 'asc',
                                width: 150
                            },

                            {
                                label: 'Priority',
                                field: 'priority',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Deadline',
                                field: 'deadline',
                                sort: 'asc',
                                width: 30
                            },
                            /*{
                                label: 'Status',
                                field: 'status',
                                sort: 'asc',
                                width: 30
                            },*/
                            {
                                label: '',
                                field: 'choose',
                                width: 30
                            }
                        ],
                        rows: table2
                    }
                });
            })
    };

    chooseOrder(id) {
        const receiver = {
            order: {
                id_order: id
            }
        }
        
        fetch(`api/Cut/Return_Package_To_Cut`, {
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
        //   .then(json => this.props.history.push('/ready_packages'))



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
        let table = this.table();
        return (

            <div>
                {table}
            </div>
        )
    }

}