import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { Link } from 'react-router-dom';
import './glassAttributes.css';
import Sidebar from '../Sidebar';

export class GlassAtributes extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },
            table2: {
                columns: [],
                rows: []
            },
        };
    }

    componentDidMount() {
        var table2 = [];
        fetch(`api/Magazine/Return_All_Colors`, {
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
                        number: i+1,
                        color:json[i],
                        edit:
                            <Link to="/glasscoloredit"><button className="glass_edit_but" id={i}
                                onClick={
                                    (e) => {
                                        console.log(e.target.id);
                                        sessionStorage.setItem('color', json[e.target.id]);
                                    }
                                }>Edytuj</button>
                            </Link>
                    })
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Nr',
                                field: 'number',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Kolor',
                                field: 'color',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Edycja',
                                field: 'edit',
                                width: 150
                            },                            
                        ],
                        rows: table2
                    }
                });
            })


        var table3 = [];
        fetch(`api/Magazine/Return_All_Type`, {
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
                    table3.push({
                        number: i + 1,
                        type: json[i],
                         edit:
                             <Link to="/glasstypeedit"><button className="glass_edit_but" id={i}
                                onClick={
                                    (e) => {
                                        console.log(e.target.id);
                                        sessionStorage.setItem('type', json[e.target.id]);
                                    }
                                }>Edytuj</button>
                            </Link>
                    })
                };
                this.setState({
                    table2: {
                        columns: [
                            {
                                label: 'Nr',
                                field: 'number',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Typ',
                                field: 'type',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Edycja',
                                field: 'edit',
                                width: 150
                            },
                        ],
                        rows: table3
                    }
                });
            })
};

    backToHome = (event) => {
        this.props.history.push('/controlpanel');
    }

    addType = (event) => {
        this.props.history.push('/add_type')
    }

    addColor = (event) => {
        this.props.history.push('/add_color')
    }


    ///funkcjie do dokończenia
    colorEdit = (event) => {
        event.prefentDefault();
        this.props.history.push('/glasscoloredit')
    }

    typeEdit = (event) => {
        event.prefentDefault();
        this.props.history.push('/glasstypeedit')
    }

    colorDel = (event) => {
        event.prefentDefault();
    }


    table() {
        return (
            <MDBDataTable
                /*striped*/
                bordered
                small
                data={this.state.table}
            />
        )
    }

    table2() {
        return (
            <MDBDataTable
                /*striped*/
                bordered
                small
                data={this.state.table2}
            />
        )
    }

    render() {
        let colorTable = this.table();
        let typeTable = this.table2();
        return (
            <div className="glassattributes">
                <Sidebar />
                <div>
                    <div className="nav_ga1">
                    </div>
                    <div className="conteiner_ga1">
                        <button type="button" className="add_color_at" onClick={this.addColor}>Dodaj kolor</button>
                        {colorTable}
                        <button type="button" className="add_type_at" onClick={this.addType}>Dodaj typ</button>
                        {typeTable}
                    </div>
                    

                
                </div>
            </div>
        )
    }


}