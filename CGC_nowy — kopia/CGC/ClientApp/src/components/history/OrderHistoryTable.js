import { Link } from 'react-router-dom';
import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import Sidebar from '../Sidebar';

export class OrderHistoryTable extends Component {
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
            order: {
                id_order:  1,
            }
        }
        console.log(receiver)
        fetch(`api/Order/Return_Order_History`, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(receiver),
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
                                label: 'Who',
                                field: 'who',
                                sort: 'asc',
                                width: 150
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
        let xd = this.table();
        return (
            <div>
                {xd}
            </div>
        )
    }

}