import { Link } from 'react-router-dom';
import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';

export class GlassHistoryTable extends Component {
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
        var table = [];

        this.setState({
            table333: {
                columns: [
                    {
                        label: 'Kto',
                        field: 'who',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Co',
                        field: 'what',
                        sort: 'asc',
                        width: 250
                    },
                    {
                        label: 'Kiedy',
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
                rows: table
            }
        });

    };


    table() {
        return (
            <MDBDataTable

                bordered
                small
                data={this.state.table333}
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