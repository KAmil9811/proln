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
        var table4 = [];
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('orderId2')
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
             
                for (var i = 0; i < json.length; i++) {
                    table3.push({
                        id: json[i].id_Order,
                        color: json[i].color,
                        owner: json[i].owner,
                        type: json[i].type,
                        thickness: json[i].thickness,
                       
                        more: <Link to="/glass_picker"> <button className="success_t" id={i}
                            onClick={(e) => {
                                sessionStorage.setItem('idOpti', table3[e.target.id].id)
                                sessionStorage.setItem('colorOpti', table3[e.target.id].color)
                                sessionStorage.setItem('typeOpti', table3[e.target.id].type)
                                sessionStorage.setItem('thicknessOpti', table3[e.target.id].thickness)
                                
                            }
                            } > Select </button></Link>
                    })
                     sessionStorage.setItem('colorpick', json[i].color)
                     sessionStorage.setItem('typepick', json[i].type)
                     sessionStorage.setItem('thicknesspick', json[i].thickness)
                    sessionStorage.setItem('ownerpick', json[i].owner) 
                      
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
                                label: 'Color',
                                field: 'color',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Client',
                                field: 'owner',
                                sort: 'asc',
                                width: 150
                            },

                            {
                                label: 'Type',
                                field: 'type',
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
                                label: '',
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
