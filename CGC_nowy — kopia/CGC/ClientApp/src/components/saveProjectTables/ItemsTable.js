import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import Sidebar from '../Sidebar';


export class ItemsTable extends Component {
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
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('orderId2'),
            },
            id: sessionStorage.getItem('cutId2'),
        }
        fetch(`api/Cut/Return_Porject`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                //console.log(json)
                return (json)
            })
            .then(json => {
                var table2 = [];
                for (var i = 0; i < json.length-1; i++) {
                    for (var j = 0; j < json[i].glass_info.length; j++) {
                        for (var x = 0; x < json[i].glass_info[j].pieces.length; x++) {
                            table2.push({
                                length: json[i].glass_info[0].pieces[x].lenght,
                                width: json[i].glass_info[0].pieces[x].widht,
                                ids: json[i].glass_info[0].pieces[x].id,
                            })

                        }


                    }
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Długość',
                                field: 'length',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Szerokość',
                                field: 'width',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Id',
                                field: 'ids',
                                sort: 'asc',
                                width: 30
                            },
                        ],
                        rows: table2
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