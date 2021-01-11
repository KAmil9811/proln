import React, { Component } from 'react';

import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import './OrderTable.css';
import Sidebar from '../Sidebar';

export class OrderTable extends Component {
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
        fetch(`api/Order/Return_All_Orders`, {
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
                        status: json[i].status,
                        klient: json[i].owner,
                        x: json[i].stan,
                        priority: json[i].priority,
                        deadline: json[i].deadline,
                        items: json[i].items,
                        more: <Link to="/oneorder"><button className="details3" id={i}
                            onClick={
                                (e) => {
                                   /* console.log(table2[e.target.id].items);*/
                                    sessionStorage.setItem('orderId', table2[e.target.id].number);
                                    sessionStorage.setItem('klient', table2[e.target.id].klient);
                                    sessionStorage.setItem('deadline', table2[e.target.id].deadline);
                                    sessionStorage.setItem('priority', table2[e.target.id].priority);
                                    
                                }
                            }>Szczegóły</button></Link>
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
                            /*{
                                label: 'Y',
                                field: 'y',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'z',
                                field: 'z',
                                sort: 'asc',
                                width: 30
                            },*/
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
                            {
                                label: 'Status',
                                field: 'status',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Więcej',
                                field: 'more',
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
            <MDBDataTableV5


                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={15}
                pagesAmount={10}
                data={this.state.table}
                searchTop


                materialSearch
                searchBottom={false}
                // barReverse
                //  pagingTop
                // scrollX
                // scrollY
                responsive
                // maxHeight="35vh"
                bordered



                //   maxHeight="20vh"
                // borderless
                // btn
                // dark


                //maxHeight="400px"

                 paginationLabel={["<", ">"]}

                sortable


                // small
                // tego w ciemnym trybie nie ruszać/ striped/
                // theadColor="indigo"
                theadTextWhite
                // theadColor="indigo"
                theadTextWhite
                // barReverse
                // className="User_table"
                // noBottomColumns
                sortable
            //info={false}


            //   autoWidth


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