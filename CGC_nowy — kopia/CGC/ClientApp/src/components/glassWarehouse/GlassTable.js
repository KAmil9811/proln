import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { Link } from 'react-router-dom';
import './GlassTable.css';


export class GlassTable extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },
            ids:'',
        };
    }



    componentDidMount() {
        var table2 = [];       
        fetch(`api/Magazine/Return_All_Glass`, {
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
                        length: json[i].length,
                        width: json[i].width,
                        thickness: json[i].hight,
                        color: json[i].color,
                        type: json[i].type,
                        amount: json[i].glass_info.length,
                        owner: json[i].owner,
                        id: '',
                        /*action: <button className="delete" id={i} onClick={
                            (e) => {
                                e.preventDefault();
                                console.log(json[e.target.id].glass_info)
                                var amount = prompt("Podaj ilość sztuk do usunięcia z przedziału od 1 do ");
                                var amount2 = parseInt(amount)


                                //TO przerobić w następnym semestrze 
                                *//*if (amount === null) {
                                    return;
                                }
                                else if (isNaN(amount2)) {
                                    alert("Proszę wprowadzić liczbę!");
                                    console.log(amount2)
                                }
                                else if (amount2 > json[e.target.id].glass_info.length) {
                                    alert("Wprowadź liczbę z odpowiedniego przedziału!")
                                }
                                else {
                                   
                                    for (var j = 0; j < amount2; j++) {
                                        this.setState({ ids: this.state.ids.concat(json[e.target.id].glass_info[j].id) })
                                        console.log(this.state.ids)
                                        }
                                   
                                   
                                    
                                }*//*
                                
                            }
                        }>Usuń</button>,*/
                        edit:
                            <Link to="/glass_edit"><button className="glass_edit" id={i}
                                onClick={
                                    (e) => {
                                        console.log(e.target.id);
                                        sessionStorage.setItem('length', json[e.target.id].length);
                                        sessionStorage.setItem('width', json[e.target.id].width);
                                        sessionStorage.setItem('thickness', json[e.target.id].hight);
                                        sessionStorage.setItem('color', json[e.target.id].color);
                                        sessionStorage.setItem('type', json[e.target.id].type);
                                        sessionStorage.setItem('amount', json[e.target.id].glass_info.length);
                                        sessionStorage.setItem('owner', json[e.target.id].owner);
                                    }
                                }>Edytuj</button>
                            </Link>

                    })
                };
                for (var k = 0; k < table2.length; k++) {
                    var amount = json[k].glass_info.length;
                    console.log(amount)
                    for (var j = 0; j < amount; j++) {
                        table2[k].id = table2[k].id + json[k].glass_info[j].id + ', '
                    }
                }
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Długość',
                                field: 'length',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Szerokość',
                                field: 'width',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Grubość',
                                field: 'thickness',
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
                                label: 'Rodzaj',
                                field: 'type',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Właściciel',
                                field: 'owner',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Ilość',
                                field: 'amount',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'ID',
                                field: 'id',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Edytuj',
                                field: 'edit',
                                sort: 'asc',
                                width: 150
                            },
                           /* {
                                label: 'Usuń',
                                field: 'action',
                                sort: 'asc',
                                width: 150
                            },*/
                            
                        ],
                        rows: table2
                    }
                });
            })
    };

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

    render() {
        let xd = this.table();
        return (

            <div>
                {xd}
            </div>
        )
    }

}