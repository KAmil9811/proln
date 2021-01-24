import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import Sidebar from '../Sidebar';


export class PackagesTable extends Component {
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
        var table3 = [];
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('orderId2')
            }
        }
        console.log(receiver)
        fetch(`api/Cut/Return_Package_To_Cut`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }

        })
            .then(res => res.json())
            .then(json => {
                console.log(json)
                for (var i = 0; i < json.length; i++) {
                    table3.push({
                        id: json[i].id_Order,
                        color: json[i].color,
                        owner: json[i].owner,
                        type: json[i].type,
                        thickness: json[i].thickness,
                        amount: json[i].item.length,
                        more: <Link to="/test"> <button className="success_t" id={i}
                            onClick={(e) => {
                                sessionStorage.setItem('idOpti', table3[e.target.id].id)
                                sessionStorage.setItem('colorOpti', table3[e.target.id].color)
                                sessionStorage.setItem('typeOpti', table3[e.target.id].type)
                                sessionStorage.setItem('thicknessOpti', table3[e.target.id].thickness)
                                
                            }
                            } > Wybierz </button></Link>
                    })
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Id',
                                field: 'id',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Kolor',
                                field: 'color',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Klient',
                                field: 'owner',
                                sort: 'asc',
                                width: 150
                            },

                            {
                                label: 'Typ',
                                field: 'type',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Grubość',
                                field: 'thickness',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Ilość szkieł',
                                field: 'amount',
                                width: 30
                            },
                             {
                                label: 'Wybierz',
                                field: 'more',
                                width: 30
                            }
                        ],
                        rows: table3
                    }
                });
            })
    }



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

                // paginationLabel={["<", ">"]}

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
