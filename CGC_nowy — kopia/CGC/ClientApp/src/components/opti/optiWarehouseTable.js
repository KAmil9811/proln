import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { Link } from 'react-router-dom';
import './test.css'


export class OptiWarehouseTable extends Component
{
    constructor(props) {
        super(props);
        this.state = {
        table:
            {
            columns:[],
                rows:[]
            },
        };
    }



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
                console.log(json.length);
                console.log(json);
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        number: json[i].id_Order,
                        //status: json[i].status,
                        klient: json[i].owner,
                        x: json[i].stan,
                        priority: json[i].priority,
                        deadline: json[i].deadline,
                        items: json[i].items,
                        choose: <Link to="/ready_packages"> <button className="choose_order" id={i}
                            onClick={(e) => {
                                //this.chooseOrder( table2[e.target.id].number);
                                sessionStorage.setItem('orderId2', table2[e.target.id].number)

                            }
                            } > Wybierz </button></Link>



                    })
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Id',
                                field: 'number',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Klient',
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
                                label: 'Priorytet',
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
                                label: 'Więcej',
                                field: 'choose',
                                width: 30
                            }
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
                data={this.state.table}
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