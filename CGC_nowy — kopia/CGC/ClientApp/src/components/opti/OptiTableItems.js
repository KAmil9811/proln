import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { Link } from 'react-router-dom';


export class OptiTableItems extends Component {
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
                for (var i = 0; i < json.length; i++) {
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