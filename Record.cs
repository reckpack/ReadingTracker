using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingTracker
{

    class Record
    {
        // basics
        int         m_id;
        string      m_name;
        string      m_book_type;
        int         m_current_read;
        int         m_latest_released;
        bool        m_is_finished;

        // details
        string      m_genre;
        string      m_publisher;
        int         m_category_id;
        DateTime    m_initial_release_date;
        DateTime    m_latest_release_date;
        string[]    m_writters;
        string[]    m_artists;

        public Record(int _id, string _name, string _bookType = "Issue", int _currentRead = 0, int _latestReleased = 0, bool _finished = false,
                        string _genre = "", string _publisher = "", int _categoryID = 0, DateTime _initialRelease = new DateTime(), DateTime _latestRelease = new DateTime(),
                        string _writters = "", string _artist = "")
        {
            m_id = _id;
            m_name = _name;
            m_book_type = _bookType;
            m_current_read = _currentRead;
            m_latest_released = _latestReleased;
            m_is_finished = _finished;
            m_genre = _genre;
            m_publisher = _publisher;
            m_category_id = _categoryID;
            m_initial_release_date = _initialRelease;
            m_latest_release_date = _latestRelease;
            m_writters = _writters.Split(',');
            m_artists = _artist.Split(',');
        }

        
    }
}
