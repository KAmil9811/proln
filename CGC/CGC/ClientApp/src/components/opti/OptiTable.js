import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import Sidebar from '../Sidebar';


export class OptiTable extends Component {
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
                id_order: sessionStorage.getItem('idOpti'),
            },
            user: {
                company: sessionStorage.getItem('company'),
                login: sessionStorage.getItem('login'),
            },
            item:  {
                color: sessionStorage.getItem('colorOpti'),
                type: sessionStorage.getItem('typeOpti'),
                thickness: sessionStorage.getItem('thicknessOpti'),

            }
        }
        fetch(`api/Cut/Magic`, {
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
                    table2.push({
                        length: json[i].length,
                        width: json[i].width,
                        thickness: json[i].hight,
                        color: json[i].color,
                        type: json[i].type,
                        ids: json[i].glass_info[0].id,
                        status: json[i].status,
                        desk: json[i].desk,
                   

                    })
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Length',
                                field: 'length',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Width',
                                field: 'width',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Thickness',
                                field: 'thickness',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Color',
                                field: 'color',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Type',
                                field: 'type',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'No.',
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